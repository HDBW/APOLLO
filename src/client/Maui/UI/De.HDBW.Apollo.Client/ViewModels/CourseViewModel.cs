// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public class CourseViewModel : BaseViewModel
    {
        private long? _courseItemId;

        public CourseViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICourseItemRepository courseItemRepository,
            ILogger logger)
            : base(
                dispatcherService,
                navigationService,
                dialogService,
                logger)
        {
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            CourseItemRepository = courseItemRepository;
        }

        private ICourseItemRepository CourseItemRepository { get; }

        public async override Task OnNavigatedToAsync()
        {
            if (!_courseItemId.HasValue)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var courseItem = await CourseItemRepository.GetItemByIdAsync(_courseItemId.Value, worker.Token).ConfigureAwait(false);

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(
                        courseItem), worker.Token);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            _courseItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }

        private void LoadonUIThread(CourseItem? courseItem)
        {
        }
    }
}
