﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using CodeHub.Helpers;
using CodeHub.Services;
using CodeHub.Views;
using Octokit;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CodeHub.ViewModels
{
    public class IssueDetailViewmodel : AppViewmodel
    {
        public Issue _issue;
        public Issue Issue
        {
            get
            {
                return _issue;
            }
            set
            {
                Set(() => Issue, ref _issue, value);

            }
        }

        public string _login;
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                Set(() => Login, ref _login, value);
            }
        }

        public string _repoName;
        public string RepoName
        {
            get
            {
                return _repoName;
            }
            set
            {
                Set(() => RepoName, ref _repoName, value);
            }
        }

        public Geometry _StateSymbol;
        public Geometry StateSymbol
        {
            get
            {
                return _StateSymbol;
            }
            set
            {
                Set(() => StateSymbol, ref _StateSymbol, value);

            }
        }
        
        public string _StateColor;
        public string StateColor
        {
            get
            {
                return _StateColor;
            }
            set
            {
                Set(() => StateColor, ref _StateColor, value);

            }
        }

        public ObservableCollection<IssueComment> _comments;
        public ObservableCollection<IssueComment> Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                Set(() => Comments, ref _comments, value);

            }
        }

        public async Task Load(Tuple<string, string, Issue> tuple)
        {
            Issue = tuple.Item3;
            Login = tuple.Item1;
            RepoName = tuple.Item2;

            if (!GlobalHelper.IsInternet())
            {
                //Sending NoInternet message to all viewModels
                Messenger.Default.Send(new GlobalHelper.LocalNotificationMessageType { Message="No Internet", Glyph= "\uE704" });
            }
            else
            {
                isLoading = true;
                Comments = await RepositoryUtility.GetAllCommentsForIssue(Login, RepoName, Issue.Number);
                isLoading = false;

            }
        }

        public void CommentTapped(object sender, ItemClickEventArgs e)
        {
            SimpleIoc.Default.GetInstance<Services.IAsyncNavigationService>().NavigateAsync(typeof(CommentsView), "Comments", e.ClickedItem as IssueComment);
        }
        private RelayCommand _userTapped;
        public RelayCommand UserTapped
        {
            get
            {
                return _userTapped
                    ?? (_userTapped = new RelayCommand(
                                          () =>
                                          {
                                              SimpleIoc.Default.GetInstance<Services.IAsyncNavigationService>().NavigateAsync(typeof(DeveloperProfileView), "Profile", Issue.User.Login);
                                          }));
            }
        }
    }
}
