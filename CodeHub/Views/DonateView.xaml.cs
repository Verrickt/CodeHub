﻿using CodeHub.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Services.Store;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using CodeHub.Helpers;
using GalaSoft.MvvmLight.Messaging;

namespace CodeHub.Views
{
    public sealed partial class DonateView : SettingsDetailPageBase
    {
        private const string donateFirstAddOnId = "[Donate_first_tier_id]";
        private const string donateSecondAddOnId = "[Donate_second_tier_id]";
        private const string donateThirdAddOnId = "[Donate_third_tier_id]";
        private const string donateFourthAddOnId = "[Donate_fourth_tier_id]";

        private static readonly StoreContext WindowsStore = StoreContext.GetDefault();

        private AppViewmodel ViewModel;
        public DonateView()
        {
            this.InitializeComponent();
            ViewModel = new AppViewmodel();
            this.DataContext = ViewModel;
        }
        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState != null)
                TryNavigateBackForDesktopState(e.NewState.Name);
        }

        private async void first_tier_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.isLoading = true;
            StorePurchaseResult result = await WindowsStore.RequestPurchaseAsync(donateFirstAddOnId);
            ViewModel.isLoading = false;
            reactToPurchaseResult(result);
        }
        private async void second_tier_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.isLoading = true;
            StorePurchaseResult result = await WindowsStore.RequestPurchaseAsync(donateSecondAddOnId);
            ViewModel.isLoading = false;
            reactToPurchaseResult(result);
        }
        private async void third_tier_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.isLoading = true;
            StorePurchaseResult result = await WindowsStore.RequestPurchaseAsync(donateThirdAddOnId);
            ViewModel.isLoading = false;
            reactToPurchaseResult(result);
        }
        private async void fourth_tier_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.isLoading = true;
            StorePurchaseResult result = await WindowsStore.RequestPurchaseAsync(donateFourthAddOnId);
            ViewModel.isLoading = false;
            reactToPurchaseResult(result);
        }

        private void reactToPurchaseResult(StorePurchaseResult result)
        {
            if (result.Status == StorePurchaseStatus.Succeeded)
            {
                Messenger.Default.Send(new GlobalHelper.LocalNotificationMessageType {
                    Message = "Thanks for your donation! I deeply appreciate your contribution to the development of CodeHub",
                    Glyph = "\uED54"
                });
            }
            else if (result.Status == StorePurchaseStatus.AlreadyPurchased)
            {
                Messenger.Default.Send(new GlobalHelper.LocalNotificationMessageType
                {
                    Message = "It seems you have already made this donation",
                    Glyph = "\uE783"
                });
            }
            else
            {
                Messenger.Default.Send(new GlobalHelper.LocalNotificationMessageType
                {
                    Message = "There seems to be a problem. Try again later",
                    Glyph = "\uE783"
                });
            }
        }
    }
}
