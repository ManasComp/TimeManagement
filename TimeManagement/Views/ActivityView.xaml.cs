using System;
using TimeManagement.Helpers;
using TimeManagement.Interfaces;
using TimeManagement.Models;
using TimeManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityView : ContentPage, IHasCollectionView
    {
        private readonly AnalyticsHelper _analyticsHelper;
        public ActivityView()
        {
            InitializeComponent();
            _analyticsHelper = new AnalyticsHelper();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _analyticsHelper.TrackEventAsync("ActivityView opened");
            //(BindingContext as ActivityViewModel)?.ToRun();
            (BindingContext as ActivityViewModel)?.FirstScroll();//why it does not work properly?
        }

        public CollectionView CollectionView => Activities_CollectionView;
        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is IHasCollectionViewModel hasCollectionViewModel)
            {
                hasCollectionViewModel.View = this;
            }
            base.OnBindingContextChanged();
        }
    }
}