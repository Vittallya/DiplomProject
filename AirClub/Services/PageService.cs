using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Concurrent;
using AirClub.ResoursesXamlAC.SlideAnimations;
using AirClub.ViewModels;
using AirClub.ViewModels.Interfaces;

namespace AirClub.Services
{
    //public class PageService1
    //{

    //    public void Subsribe<TPage>(TPage page, IWindowViewModel viewModel, Action<Page, bool, ISlideAnimation> action) where TPage: Page
    //    {
    //        if(subsribers == null)
    //        {
    //            subsribers = new ConcurrentDictionary<IWindowViewModel, Action<Page, bool, ISlideAnimation>>();
    //        }

    //        subsribers.TryAdd(viewModel, action);
    //        mainPages.TryAdd(viewModel, page.GetType());
    //        history.TryAdd(viewModel, new List<Page>());
    //    }

    //    private ConcurrentDictionary<IWindowViewModel, Action<Page, bool, ISlideAnimation>> subsribers;

    //    public void ChangePage(IWindowViewModel viewModel, Page page, ISlideAnimation slideAnimation = null)
    //    {
    //        bool needBack = true;

    //        if ( mainPages[viewModel] == page.GetType() )
    //        {
    //            if (history.ContainsKey(viewModel))
    //            {
    //                history[viewModel].Remove(page);
    //            }
    //            needBack = false;

    //        }


    //        if (history.ContainsKey(viewModel) && !history[viewModel].Contains(page) )
    //        {
    //            history[viewModel].Add(page);
    //        }

    //        var action = subsribers.Where(x => x.Key == viewModel).Select(x => x.Value).FirstOrDefault();
    //        action?.Invoke(page, needBack, slideAnimation);

    //    }

    //    public void Back(IWindowViewModel viewModel)
    //    {
    //        var currentHistory = history.Where(x => x.Key == viewModel).Select(x => x.Value).FirstOrDefault();

    //        Page backPage = currentHistory[currentHistory.Count - 2];
    //        Page currPage = currentHistory[currentHistory.Count - 1];

    //        history[viewModel].Remove(currPage);

    //        ChangePage(viewModel, backPage, new SlideAnimationMove(AnimSlideDir.Right, 0.5, TranslateTransform.XProperty));
    //    }

    //    private ConcurrentDictionary<IWindowViewModel, IList<Page>> history = new ConcurrentDictionary < IWindowViewModel, IList<Page>>();

    //    private ConcurrentDictionary<IWindowViewModel, Type> mainPages = new ConcurrentDictionary<IWindowViewModel, Type>();

    //}

    public class PageService
    {
        ConcurrentDictionary<IWindowViewModel, IViewModelSubscriber> viewModels = 
            new ConcurrentDictionary<IWindowViewModel, IViewModelSubscriber>();


        public void Register(IWindowViewModel windowViewModel, Action<Page, bool, ISlideAnimation> action, params Type[] mainPages)
        {
            if (viewModels.ContainsKey(windowViewModel))
            {
                viewModels[windowViewModel] = new ViewModelSubscriber(action, mainPages);
            }
            else
            {
                viewModels.TryAdd(windowViewModel, new ViewModelSubscriber(action, mainPages));
            }
        }

        public void ChangePage(IWindowViewModel viewModel, Page page, ISlideAnimation slideAnimation = null, bool? needBack = null)
        {
            if(viewModel == null)
            {
                viewModels.
                    Where(x => x.Key.GetType() == typeof(MainViewModel)).
                    Select(x => x.Value).
                    SingleOrDefault()?.ChangePage(page, slideAnimation, needBack);
            }

            else if (viewModels.ContainsKey(viewModel))
            {
                viewModels[viewModel].ChangePage(page, slideAnimation, needBack);
            }
        }

        public void GoToPage<TPage>(IWindowViewModel viewModel) where TPage: Page, new()
        {
            if(viewModel == null)
            {
                viewModels.
                    Where(x => x.Key.GetType() == typeof(MainViewModel)).
                    Select(x => x.Value).
                    SingleOrDefault()?.GoToPage<TPage>();
            }
            else
            {
                viewModels[viewModel].GoToPage<TPage>();
            }
        }

        public void Back(IWindowViewModel viewModel)
        {
            if(viewModel == null)
            {
                viewModels.
                    Where(x => x.Key.GetType() == typeof(MainViewModel)).
                    Select(x => x.Value).
                    SingleOrDefault()?.Back();
            }
            else if (viewModels.ContainsKey(viewModel))
            {
                viewModels[viewModel].Back();
            }
        }
        public void RemoveViewModel(IWindowViewModel viewModel)
        {
            if (viewModels.ContainsKey(viewModel))
            {
                viewModels.TryRemove(viewModel, out var _);
            }
        }

        public void ClearAllHistory(IWindowViewModel viewModel = null)
        {
            if (viewModels.ContainsKey(viewModel))
            {
                viewModels[viewModel].History.Clear();
            }
            else
            {
                foreach (var vm in viewModels)
                {
                    vm.Value.History.Clear();
                }
            }

        }

        public void ClearAllHistory<TViewModel>() where TViewModel: MainViewModel
        {
            viewModels.
                    Where(x => x.Key.GetType() == typeof(TViewModel)).
                    Select(x => x.Value).
                    FirstOrDefault()?.History.Clear();

        }


    }


    public interface IViewModelSubscriber
    {
        Action<Page, bool, ISlideAnimation> Action { get; }

        Dictionary<Page, ISlideAnimation> History { get; }
        IList<Type> PagesBackNotNeed { get; }

        void ChangePage(Page page, ISlideAnimation slideAnimation = null, bool? needBack = null);

        public void Back();
        public void GoToPage<TPage>() where TPage: Page, new();
    }

    public class ViewModelSubscriber: IViewModelSubscriber
    {
        public Action<Page, bool, ISlideAnimation> Action { get; }

        public Dictionary<Page, ISlideAnimation> History { get; private set; } = new Dictionary<Page, ISlideAnimation>();

        public IList<Type> PagesBackNotNeed { get; } = new List<Type>();


        public ViewModelSubscriber(Action<Page, bool, ISlideAnimation> action, IList<Type> backNotNeed)
        {
            Action = action;
            PagesBackNotNeed = backNotNeed;
        }
        
        public void ChangePage(Page page, ISlideAnimation slideAnimation = null, bool? needBack = null)
        {
            if (!History.ContainsKey(page))
            {
                History.Add(page, slideAnimation);
            }

            if (!needBack.HasValue)
            {
                needBack = History.Count > 1;
            }

            

            Action?.Invoke(page, needBack.Value, slideAnimation);
        }

        public void GoToPage<TPage>() where TPage: Page, new()
        {
            var page = History.SingleOrDefault(x => x.GetType() == typeof(TPage)).Key;

            if(page == null)
            {
                page = new TPage();
            }

            ChangePage(page);
        }

        public void Back()
        {

            var lastAnim = History.LastOrDefault().Value?.Invert();

            History.Remove(History.Last().Key); //Удаляем последнюю

            var current = History.LastOrDefault();

            Page curPage = current.Key; //перемещаемся на предпоследнюю

            bool needBack = History.Count > 1;

            Action?.Invoke(curPage, needBack, lastAnim);
        }
    }
}
