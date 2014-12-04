using Moq;
using GalaSoft.MvvmLight;
using NUnit.Framework;
using TaskMaster.Model;
using TaskMaster.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace TaskMaster.Test.MainViewModelTests
{
    [TestFixture]
    public class ConstructionTests
    {
        [Test]
        public void a_Given_a_TaskListRetrieval_service_that_returns_a_null_task_list_when_the_MainWindowModel_is_constructed_then_the_service_is_called_and_no_error_happens()
        {
            // Given
            var taskListServiceMock = new Mock<ITaskListDataService>();
            var taskPlayerMock = new Mock<ITaskPlayer>();
            taskListServiceMock.Setup(x => x.GetActiveTasks()).Returns(() => null);
            var timeProviderMock = new Mock<ITimeProvider>();

            // When
            var mainViewModel = new MainViewModel(taskListServiceMock.Object, taskPlayerMock.Object, timeProviderMock.Object);

            // Then
            taskListServiceMock.Verify(x => x.GetActiveTasks(), Times.Once());
        }

        [Test]
        public void b_Given_a_TaskListRetrieval_service_that_returns_a_task_list_when_the_MainWindowModel_is_constructed_then_the_service_is_called_and_the_task_list_is_populated()
        {
            // Given
            var taskListServiceMock = new Mock<ITaskListDataService>();
            var taskPlayerMock = new Mock<ITaskPlayer>();
            var taskList = new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).Build(),
                new TaskItemBuilder(Guid.NewGuid()).Build()
            };

            taskListServiceMock.Setup(x => x.GetActiveTasks()).Returns(taskList);
            var timeProviderMock = new Mock<ITimeProvider>();

            // When
            var mainViewModel = new MainViewModel(taskListServiceMock.Object, taskPlayerMock.Object, timeProviderMock.Object);
            while (mainViewModel.ActiveTaskList == null) ; // wait for the async bgworker to complete

            // Then
            taskListServiceMock.Verify(x => x.GetActiveTasks(), Times.Once());
            Assert.AreEqual(mainViewModel.ActiveTaskList, new ObservableCollection<TaskItem>(taskList));
        }

        [Test]
        public void c_Given_a_TaskListRetrieval_service_that_returns_an_empty_archived_task_list_when_the_MainViewModel_is_constructed_then_the_service_is_called_and_no_error_happens()
        {
            // Given
            var taskListServiceMock = new Mock<ITaskListDataService>();
            var taskPlayerMock = new Mock<ITaskPlayer>();
            taskListServiceMock.Setup(x => x.GetRecentArchivedTasks()).Returns(new List<TaskItem>());
            var timeProviderMock = new Mock<ITimeProvider>();

            // When
            var mainViewModel = new MainViewModel(taskListServiceMock.Object, taskPlayerMock.Object, timeProviderMock.Object);

            // Then
            taskListServiceMock.Verify(x => x.GetRecentArchivedTasks(), Times.Once());
        }
    }
}
