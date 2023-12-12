// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Client.Models.Course
{
    public partial class CourseItemEntry : ObservableObject
    {
        private readonly CourseItem _courseItem;

        private Func<CourseItemEntry, bool> _canOpenCourseItemHandle;

        private Func<CourseItemEntry, Task> _openCourseItemHandler;

        private CourseItemEntry(CourseItem courseItem, Func<CourseItemEntry, Task> openCourseItemHandler, Func<CourseItemEntry, bool> canOpenCourseItemHandle)
        {
            ArgumentNullException.ThrowIfNull(courseItem);
            _courseItem = courseItem;
            _canOpenCourseItemHandle = canOpenCourseItemHandle;
            _openCourseItemHandler = openCourseItemHandler;
        }

        public string DisplayTitle
        {
            get
            {
                return _courseItem.Title ?? string.Empty;
            }
        }

        public static CourseItemEntry Import(CourseItem courseItem, Func<CourseItemEntry, Task> openCourseItemHandler, Func<CourseItemEntry, bool> canOpenCourseItemHandle)
        {
            return new CourseItemEntry(courseItem, openCourseItemHandler, canOpenCourseItemHandle);
        }

        public CourseItem Export()
        {
            return _courseItem;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenCourseItem), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private Task OpenCourseItem(CancellationToken token)
        {
            return _openCourseItemHandler?.Invoke(this) ?? Task.CompletedTask;
        }

        private bool CanOpenCourseItem()
        {
            return _canOpenCourseItemHandle?.Invoke(this) ?? false;
        }
    }
}
