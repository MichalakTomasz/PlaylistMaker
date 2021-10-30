using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlaylistMaker.Commons.Behaviors
{
    public class SliderValueChangedBehavior : Behavior<Slider>, ICommandSource
    {
        protected override void OnAttached()
        {
            AssociatedObject.ValueChanged += AssociatedObject_ValueChanged;
        }

        private void AssociatedObject_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            => ExecuteCommand();

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SliderValueChangedBehavior), new PropertyMetadata(null, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as SliderValueChangedBehavior;
            if (slider != default)
            {
                ICommand oldCommad = e.OldValue as ICommand;
                if (oldCommad != default)
                    oldCommad.CanExecuteChanged -= slider.CanExecuteChanged;
                ICommand newValue = e.NewValue as ICommand;
                if (newValue != default)
                    newValue.CanExecuteChanged += slider.CanExecuteChanged;
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command != default)
            {
                RoutedCommand command = Command as RoutedCommand;
                if (command != default)
                {
                    if (command.CanExecute(CommandParameter, CommandTarget))
                        AssociatedObject.IsEnabled = true;
                    else
                        AssociatedObject.IsEnabled = false;
                }
                else
                {
                    if (Command.CanExecute(CommandParameter))
                        AssociatedObject.IsEnabled = true;
                    else
                        AssociatedObject.IsEnabled = false;
                }
            }
        }

        private void ExecuteCommand()
        {
            if (Command != default)
            {
                var command = Command as RoutedCommand;
                if (command != default)
                    command.Execute(CommandParameter, CommandTarget);
                else Command.Execute(CommandParameter);
            }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(SliderValueChangedBehavior), new PropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(SliderValueChangedBehavior));
    }
}
