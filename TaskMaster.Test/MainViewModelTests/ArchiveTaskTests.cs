using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMaster.Model;

namespace TaskMaster.Test.MainViewModelTests
{
    [TestFixture]
    public class ArchiveTaskTests
    {

        [Test]
        public void Given_a_task_from_the_active_list_when_the_ArchiveTask_command_is_handled_then_the_data_service_is_called_and_the_task_is_removed_from_the_Active_task_list_and_it_is_added_to_the_Archived_task_list()
        {
            // Given
            Mock<ITaskListDataService> taskListDataServiceMock;
            var sut = MVMHelpers.SutWithTaskList(out taskListDataServiceMock);
            var taskToArchive = sut.ActiveTaskList[0];

            taskListDataServiceMock.Setup(x => x.ArchiveTask(taskToArchive)).Returns(ArchiveTaskResult.Ok);

            // When
            sut.ArchiveTaskItemCmd.Execute(taskToArchive.Id);

            // Then
            Assert.IsTrue(! sut.ActiveTaskList.Contains(taskToArchive));
            Assert.IsTrue(sut.ArchivedTaskList.Contains(taskToArchive));
            taskListDataServiceMock.Verify(x => x.ArchiveTask(taskToArchive), Times.Once());
        }

        [Test]
        public void Given_a_collection_of_archived_tasks_and_a_task_from_the_active_list_when_the_ArchiveTask_command_is_handled_then_the_task_is_added_as_the_first_element_of_the_Archived_task_list()
        {
            // Given
            var sut = MVMHelpers.SutWithActiveAndArchivedTaskLists();
            var taskToArchive = sut.ActiveTaskList[0];

            var previousArchivedTasksCount = sut.ArchivedTaskList.Count();

            // When
            sut.ArchiveTaskItemCmd.Execute(taskToArchive.Id);

            // Then
            Assert.IsTrue(previousArchivedTasksCount + 1 == sut.ArchivedTaskList.Count());
            Assert.IsTrue(sut.ArchivedTaskList.First().Id == taskToArchive.Id);
        }

        [Test]
        public void Given_a_task_from_the_active_list_and_a_service_that_is_not_responding_when_the_ArchiveTask_command_is_handled_then_the_task_is_not_moved_between_lists()
        {
            // Given
            Mock<ITaskListDataService> taskListDataServiceMock;
            var sut = MVMHelpers.SutWithTaskList(out taskListDataServiceMock);
            var taskToArchive = sut.ActiveTaskList[0];

            taskListDataServiceMock.Setup(x => x.ArchiveTask(taskToArchive)).Returns(ArchiveTaskResult.Error);

            // When
            sut.ArchiveTaskItemCmd.Execute(taskToArchive.Id);

            // Then
            Assert.IsFalse(!sut.ActiveTaskList.Contains(taskToArchive));
            Assert.IsTrue(sut.ArchivedTaskList == null);
        }

        [Test]
        public void Given_a_taskId_that_is_not_in_the_active_list_when_ArchiveTaskCmd_is_handled_it_does_nothing()
        {
            // Given
            var sut = MVMHelpers.SutWithTaskList();

            // Then
            sut.ArchiveTaskItemCmd.Execute(Guid.NewGuid());
        }

        [Test]
        public void Given_a_null_active_list_when_ArchiveTaskCmd_is_handled_it_does_nothing()
        {
            // Given
            var sut = MVMHelpers.SutWithNullTaskLists();

            // Then
            sut.ArchiveTaskItemCmd.Execute(Guid.NewGuid());
        }

        [Test]
        public void Given_a_playing_task_when_ArchiveTaskCmd_is_handled_for_that_task_then_it_is_paused()
        {
            // Given
            Mock<ITaskPlayer> taskPlayerMock;
            var sut = MVMHelpers.SutWithTaskListAndPlayerMock(out taskPlayerMock);
            var playingTask = sut.ActiveTaskList[0];
            playingTask.PlayingState = PlayingState.Playing;
            sut.TaskPlayer.Play(playingTask);

            // When
            sut.ArchiveTaskItemCmd.Execute(playingTask.Id);

            // Then
            taskPlayerMock.Verify(x => x.Pause(playingTask), Times.Once());
        }
    }
}
