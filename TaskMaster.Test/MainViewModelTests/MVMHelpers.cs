using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMaster.Model;
using TaskMaster.ViewModel;

namespace TaskMaster.Test
{
    public static class MVMHelpers
    {

        public static MainViewModel SutWithNullTaskList()
        {
            var taskListServiceMock = new Mock<ITaskListDataService>();
            var taskPlayerMock = new Mock<ITaskPlayer>();
            taskListServiceMock.Setup(x => x.GetUnarchivedTasks());
            var timeProviderMock = new Mock<ITimeProvider>();
            var mainViewModel = new MainViewModel(taskListServiceMock.Object, taskPlayerMock.Object, timeProviderMock.Object);
            return mainViewModel;
        }

        public static MainViewModel SutWithTaskList()
        {
            Mock<ITaskPlayer> taskPlayerMock = null;
            var sut = SutWithTaskListAndPlayerMock(out taskPlayerMock);

            return sut;
        }

        public static MainViewModel SutWithTaskList(out Mock<ITaskListDataService> taskListServiceMock)
        {
            var taskPlayerMock = new Mock<ITaskPlayer>();
            taskListServiceMock = new Mock<ITaskListDataService>();

            var taskList = new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).Build(),
                new TaskItemBuilder(Guid.NewGuid()).Build()
            };

            taskListServiceMock.Setup(x => x.GetUnarchivedTasks()).Returns(taskList);

            var timeProviderMock = new Mock<ITimeProvider>();
            timeProviderMock.Setup(x => x.Now()).Returns(DateTime.Now);

            var sut = new MainViewModel(taskListServiceMock.Object, taskPlayerMock.Object, timeProviderMock.Object);
            while (sut.UnarchivedTaskList == null) ; // wait for the async bgworker to complete

            return sut;
        }

        public static MainViewModel SutWithTaskListAndPlayerMock(out Mock<ITaskPlayer> taskPlayerMock)
        {
            return SutWithTaskListAndPlayerMock(out taskPlayerMock, DateTime.Now);
        }

        public static MainViewModel SutWithTaskListAndPlayerMock(out Mock<ITaskPlayer> taskPlayerMock, DateTime playingDate)
        {
            var taskListServiceMock = new Mock<ITaskListDataService>();
            taskPlayerMock = new Mock<ITaskPlayer>();

            var taskList = new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).Build(),
                new TaskItemBuilder(Guid.NewGuid()).Build()
            };

            taskListServiceMock.Setup(x => x.GetUnarchivedTasks()).Returns(taskList);

            var timeProviderMock = new Mock<ITimeProvider>();
            timeProviderMock.Setup(x => x.Now()).Returns(playingDate);

            var sut = new MainViewModel(taskListServiceMock.Object, taskPlayerMock.Object, timeProviderMock.Object);
            while (sut.UnarchivedTaskList == null) ; // wait for the async bgworker to complete

            return sut;
        }

        public static void SutWithFirstPlayableTask(out MainViewModel sut, out TaskItem task)
        {
            SutWithFirstTaskPlaySetup(out sut, out task, true);
        }

        public static void SutWithFirstTaskPlaySetup(out MainViewModel sut, out TaskItem task, bool playable)
        {
            Mock<ITaskPlayer> taskPlayerMock = null;
            sut = SutWithTaskListAndPlayerMock(out taskPlayerMock);
            var task_aux = sut.UnarchivedTaskList[0];
            taskPlayerMock.Setup(x => x.CanPlay(task_aux)).Returns(playable);
            task = task_aux;
        }

        public static void SutWithFirstPausableTask(out MainViewModel sut, out TaskItem task)
        {
            SutWithFirstTaskPauseSetup(out sut, out task, true);
            sut.UnarchivedTaskList[0].PlayingState = PlayingState.Playing;
        }

        public static void SutWithFirstNoPausableAndPlayingTask(out MainViewModel sut, out TaskItem task)
        {
            SutWithFirstTaskPauseSetup(out sut, out task, false);
            sut.UnarchivedTaskList[0].PlayingState = PlayingState.Playing;
        }

        public static void SutWithFirstTaskPauseSetup(out MainViewModel sut, out TaskItem task, bool pausable)
        {
            Mock<ITaskPlayer> taskPlayerMock = null;
            sut = SutWithTaskListAndPlayerMock(out taskPlayerMock);
            var task_aux = sut.UnarchivedTaskList[0];
            taskPlayerMock.Setup(x => x.CanPause(task_aux)).Returns(pausable);
            task = task_aux;
        }

    }
}
