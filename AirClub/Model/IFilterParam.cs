using AirClub.Model.Db;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirClub.Model
{
    public interface IFilterParam<Db> where Db:  class, IDbClass, new()
    {
        public string Name { get; set; }

        public Control Control { get; set; }

        public Func<Db, bool> CollectionPredicate { get; }
        public RoutedEventHandler FilterChanged { get; set; }

        public object GetControlValue();

        public bool IsControlEnabled { get; set; }

        public void SetControlValue(object value);

        public ICommand Clear { get; }

        public bool HasPredicate { get; }

        public void OnClearButtonClicked();
    }

    public class FilterParam<T, TControl> : BindableBase, IFilterParam<T> where T : class, IDbClass, new()
        where TControl: Control
    {
        public string Name { get; set; }
        public Control Control { get; set; }
        private void DefineControlEvent(Control c)
        {
            if ( c is ComboBox combo )
            {
                combo.SelectionChanged += OnFilterChanged;
            }
            if ( c is TextBox tb )
            {
                tb.TextChanged += OnFilterChanged;
            }
            if ( c is DatePicker dt )
            {
                dt.SelectedDateChanged += OnFilterChanged;
            }
        }
        private void OnFilterChanged(object sender, RoutedEventArgs e)
        {
            FilterChanged?.Invoke(sender, e);
        }

        private bool isControlEnabled = true;

        public bool IsControlEnabled 
        {
            get => isControlEnabled;
            set { isControlEnabled = value; Control.IsEnabled = value; }
        }


        public bool HasPredicate => FilterPredicate != null;
        //TextBox, ComboBox, DatePicker, CheckBox, RadioButton (- вместо последнего используется combobox)
        //Если Combobox, то элементы указываются в свойстве Items

        private Func<TControl, object> GetValuePredicateDefault()
        {
            Func<TControl, object>  func = c =>
            {
                if (c is TextBox tb)
                {
                    return tb.Text;
                }
                else if(c is ComboBox cb)
                {
                    return cb.SelectedItem;
                }
                else if(c is DatePicker dt)
                {
                    return dt.SelectedDate;
                }
                return "";
            };
            return func;
        }

        public Func<T, bool> CollectionPredicate => item =>
        {
            if(ValuePredicate == null)
            {
                ValuePredicate = GetValuePredicateDefault();
            }

            var value = ValuePredicate.Invoke(Control as TControl);



            if (typeof(TControl) == typeof(TextBox))
            {
                if (value is string str && str.Contains('|'))
                {
                    var arr = str.Split('|');

                    foreach (var word in arr)
                    {
                        if (FilterPredicate?.Invoke(item, word) ?? true)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            return FilterPredicate?.Invoke(item, value) ?? true;
        };

        public Func<TControl, object> ValuePredicate { get; set; }


        public Func<T, object, bool> FilterPredicate { get; set; }

        public RoutedEventHandler FilterChanged { get; set; }

        public object GetControlValue()
        {
            if ( Control is ComboBox combo )
            {
                return combo.SelectedItem;
            }
            if ( Control is TextBox tb )
            {
                return tb.Text;
            }
            if ( Control is DatePicker dt && dt.SelectedDate != null)
            {
                return dt.SelectedDate.Value;
            }
            return null;
        }

        public void SetControlValue(object value)
        {
            if ( Control is ComboBox combo )
            {
                combo.SelectedItem = value;
            }
            if ( Control is TextBox tb )
            {
                tb.Text = value?.ToString();
            }
            if ( Control is DatePicker dt )
            {
                dt.SelectedDate = (DateTime?) value;
            }
        }

        public FilterParam(): this(Activator.CreateInstance(typeof(TControl)) as TControl)
        {

        }

        public FilterParam(TControl control)
        {
            Control = control;
            DefineControlEvent(control);
        }

        public ICommand Clear => new DelegateCommand(() =>
        {
            if (IsControlEnabled)
            {
                OnClearButtonClicked();
            }
        }, IsControlEnabled);

        public void OnClearButtonClicked()
        {
            SetControlValue(null);
        }
    }
}
