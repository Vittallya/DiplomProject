using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AirClub.Services;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Data;
using AirClub.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using AirClub.Model.Db;
using AirClub.Model.Access;
using AirClub.ViewModels.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace AirClub.ViewModels
{
    public interface IEditItemsBaseViewModel
    {
        Page GetPage();
        string Title { get; }
    }


    public abstract class ElementsBaseViewModel<TDbClass>: BindableBase, IEditItemsBaseViewModel where TDbClass: class, IDbClass, new()
    {

        public abstract ObservableCollection<TDbClass> Elements { get; set; }
        public TDbClass SelectedElement { get; set; }

        protected readonly PageService _pageService;
        private readonly EventBus eventBus;
        protected readonly ICurrentUserService _currentUser;
        protected AirClubDbContext _dbContext;

        protected TDbClass ItemType { get; set; } = new TDbClass();
        public abstract int DataTemplateIndex { get; }
        public virtual bool IsGenerateDataView { get; } = true;
        public bool IsSelectionMode { get; set; }

        private bool _needReload;
        public abstract string Title { get; }

        public abstract Dictionary<string, string> BindingHeaders { get; }
        public abstract Task Reload();

        public abstract Page CurrentView { get; }

        public abstract IEditItemBase<TDbClass> EditElementViewModel { get; }

        protected Window _window;

        protected IWindowViewModel _WindowViewModel;

        public abstract TDbClass ItemCopy { get; }

        public virtual bool ShowSlectionMode(out TDbClass outElement, int? filterIndex = null, object filterValue = null)
        {
            IsSelectionMode = true;
            outElement = null;
            _WindowViewModel = AirClubLocator.ServiceProvider.GetRequiredService<IWindowViewModel>();

            var page = GetPage();

            if (page != null)
            {
                _window = _WindowViewModel.GetWindow(page);

                if (filterIndex.HasValue)
                {
                    try
                    {
                        FilterParams[filterIndex.Value].SetControlValue(filterValue);
                        FilterParams[filterIndex.Value].IsControlEnabled = false;
                        //RefreshFilter();
                    }
                    catch (Exception) { }
                }

                bool res = _window.ShowDialog() ?? false;

                outElement = res ? SelectedElement : null;

                return res;
            }

            return false;
        }
        



        public bool? Result { get; private set; }

        public async Task<bool> WaitResult()
        {
            while (!Result.HasValue)
            {
                await Task.Delay(10);
            };

            return Result.Value;
        }

        public Page GetPage()
        {
            //ViewModel к моменту авторизации уже должна быть адаптирована под пользователя

            if (!HasAccessHere)
            {
                MessageBox.Show("У Вас нет доступа к этому разделу!", "Система", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return null;
            }

            OnReload();


            return CurrentView;
        }


        #region Анимация

        public Visibility LoadingVisibility { get; set; } = Visibility.Collapsed;

        public double Alpha { get; set; } = 1;

        protected virtual async Task FadeOut()
        {
            for (Alpha = 1; Alpha > 0; Alpha -= 0.1)
            {
                await Task.Delay(20);
            }
        }

        #endregion

        protected void GenerateDataView()
        {
            GridView.Columns.Clear();
            foreach(var bind in BindingHeaders)
            {
                GridViewColumn item = new GridViewColumn();

                var binding = new Binding();
                binding.Path = new PropertyPath(bind.Key);

                item.DisplayMemberBinding = new Binding(bind.Key);
                item.Header = bind.Value;
                GridView.Columns.Add(item);
            }

        }

        public GridView GridView { get; set; } = new GridView();

        #region Фильтр

        protected virtual string[] PropertiesNonUsedInFilter { get; } = new string[0];

        public virtual IList<IFilterParam<TDbClass>> FilterParams { get; private set; } = new List<IFilterParam<TDbClass>>();

        private bool HasFilterValue(out IList<IFilterParam<TDbClass>> list)
        {
            list = FilterParams.Where(x => x.HasPredicate && x.GetControlValue() != null).ToList();
            return list.Count > 0;
        }

        public async void RefreshFilter()
        {            
            Elements = await GetCollectionDefault();
            _needReload = false;

            if(HasFilterValue(out IList<IFilterParam<TDbClass>> list))
            {
                foreach (var param in list)
                {
                    var value = param.GetControlValue();
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        Elements = new ObservableCollection<TDbClass>(Elements.Where(param.CollectionPredicate));
                        _needReload = true;
                    }
                }
            }

            
        }

        public abstract Task <ObservableCollection<TDbClass>> GetCollectionDefault();

        #endregion

        

        public ElementsBaseViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _userService, AirClubDbContext dbContext)
        {
            this._pageService = _pageService;
            eventBus = _eventBus;
            _currentUser = _userService;
            _dbContext = dbContext;
            //GenerateFilter();
            if (IsGenerateDataView)
            {
                GenerateDataView();
            }

        }

        #region Взаимодействие с вид-моделью редактирования

        protected virtual void ViewElement(TDbClass item)
        {
            if(item == null)
            {
                return;
            }

            EditElementViewModel.ShowCurrent(_WindowViewModel, item, ShowMode.View);          
        }

        protected async virtual Task OnEditElement(TDbClass element)
        {
            if (element == null)
            {
                return;
            }

            var vm = EditElementViewModel;
            var copy = element.Clone() as TDbClass;

            if (await vm.ShowCurrent(_WindowViewModel, copy, ShowMode.Edit))
            {
                _needReload = vm.NeedReload ? vm.NeedReload : _needReload;
                //await EditElementProcess(element);

                element.UpdateData(copy);

                await _dbContext.SaveChangesAsync();

                if (_needReload)
                {
                    OnReload();
                }

            }
            
        }
        protected async virtual void OnAddElement()
        {
            var copy = ItemCopy;
            if (await EditElementViewModel.ShowCurrent(_WindowViewModel, copy, ShowMode.Add) )
            {
                await AddElementProcess(copy, _dbContext);
                await _dbContext.SaveChangesAsync();

                if (_needReload)
                {
                    OnReload();
                }
            }           
        }

        protected async virtual void OnRemoveElement(TDbClass[] elements)
        {
            if (elements == null || elements.Length == 0)
            {
                return;
            }
            if (MessageBox.Show("Подтвердить удаление?", "Аэроклуб", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await RemoveElementsProcess(elements, _dbContext);
                await _dbContext.SaveChangesAsync();

                if (_needReload)
                {
                    OnReload();
                }
            }
        }

        private bool _filterReady;

        private void CheckFilter()
        {
            if (!_filterReady)
            {
                foreach(var param in FilterParams)
                {
                    param.FilterChanged = (a, b) => RefreshFilter();
                }
                _filterReady = true;
            }
        }

        protected async virtual void OnReload()
        {
            ReloadVisibility();

            CheckFilter();

            if (CanReadEdit >= 1)
            {
                Elements = null;

                LoadingVisibility = Visibility.Visible;

                await Reload();
                await _currentUser.OnItemsViewModelOpen(this);

                await FadeOut();
                RefreshFilter();

                LoadingVisibility = Visibility.Collapsed;
            }
        }


        #endregion

        #region Методы-процесссы 


        public virtual async Task RemoveElementsProcess(TDbClass[] elements, AirClubDbContext db)
        {
            _dbContext.RemoveRange(elements);
            
        }
        public virtual async Task EditElementProcess(TDbClass element)
        {
            await Task.Run(() =>
            {
                _dbContext.Update(element);
            });
        }


        public virtual async Task AddElementProcess(TDbClass element, AirClubDbContext db)
        {
            await _dbContext.AddAsync(element);
        }

        public virtual void OnSelect()
        {
            if (_window != null && SelectedElement != null)
            {
                _window.DialogResult = true;
            }
        }
        #endregion

        #region Отображение элементов
        public virtual Visibility AddElementVisibility { get; set; } = Visibility.Collapsed;
        public virtual Visibility EditElementVisibility { get; set; } = Visibility.Collapsed;
        public virtual Visibility ViewElementVisibility { get; set; } = Visibility.Collapsed;
        public virtual Visibility RemoveElementVisibility { get; set; } = Visibility.Collapsed;
        public virtual Visibility SelectElementVisibility => IsSelectionMode ? Visibility.Visible : Visibility.Collapsed;

        public virtual Visibility FilterVisibility => ListBoxVisibility;

        public virtual Visibility ListBoxVisibility { get; set; } = Visibility.Collapsed;

        protected virtual void ReloadVisibility()
        {
            if (CanAdd == 1)
            {
                AddElementVisibility = Visibility.Visible;
            }
            else
            {
                AddElementVisibility = Visibility.Collapsed;
            }

            if (CanReadEdit == 0)
            {
                EditElementVisibility = Visibility.Collapsed;
                ViewElementVisibility = Visibility.Collapsed;
                RemoveElementVisibility = Visibility.Collapsed;
                ListBoxVisibility = Visibility.Collapsed;
            }

            else if (CanReadEdit == 1)
            {
                ViewElementVisibility = Visibility.Visible;
                EditElementVisibility = Visibility.Collapsed;
                RemoveElementVisibility = Visibility.Collapsed;
                ListBoxVisibility = Visibility.Visible;
            }
            else if (CanReadEdit >= 2)
            {
                ViewElementVisibility = Visibility.Collapsed;
                EditElementVisibility = Visibility.Visible;
                RemoveElementVisibility = Visibility.Visible;
                ListBoxVisibility = Visibility.Visible;

            }
        }

        #endregion

        #region Кнопки управления


        public ICommand UpdateTable => new DelegateCommand(() =>
        {
            OnReload();
        });

        public ICommand Select => new DelegateCommand(() =>
        {
            OnSelect();
        }, () => SelectedElement != null);

        public ICommand ElementDoubleClicked => new DelegateCommand(() =>
        {
            if (CanReadEdit > 1)
            {
                EditSelectedElement?.Execute(null);
            }
            else if (CanReadEdit == 1)
            {
                EditSelectedElement?.Execute(null);
            }
        });

        public ICommand EditSelectedElement => new DelegateCommand(async () =>
        {
            await OnEditElement(SelectedElement);
        }, () => SelectedElement != null);

        public ICommand AddNewElement => new DelegateCommand(() =>
        {            
            OnAddElement();            
        });

        public ICommand RemoveSelectedElement => new DelegateCommand<System.Collections.IList>(items =>
        {
            if (items != null)
            {
                OnRemoveElement(items.OfType<TDbClass>().ToArray());
            }
            else
            {
                OnRemoveElement(new[] { SelectedElement });
            }
        }, a => SelectedElement != null);


        public ICommand ViewSelectedElement => new DelegateCommand(() =>
        {
            ViewElement(SelectedElement);
        }, () => SelectedElement != null);
        #endregion

        #region параметры доступа
        public virtual int CanAdd => _currentUser.GetUserPossibility(CanAddIndex) ?? 0;

        public virtual int CanReadEdit => _currentUser.GetUserPossibility(CanReadEditIndex) ?? 0;

        public abstract bool HasAccessHere { get; }

        public abstract AccessSectionsIndex CanAddIndex { get; }

        public abstract AccessSectionsIndex CanReadEditIndex { get; }

        #endregion
    }
}
