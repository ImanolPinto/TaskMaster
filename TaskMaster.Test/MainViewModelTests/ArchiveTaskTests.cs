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
        public void Given_a_task_guid_from_the_active_list_when_the_ArchiveTask_command_is_handled_then_the_task_is_removed_from_the_Active_task_list()
        {
            // Given
            Mock<ITaskListDataService> taskListDataServiceMock;
            var sut = MVMHelpers.SutWithTaskList(out taskListDataServiceMock);
            var guidToArchive = sut.ActiveTaskList[0].Id;
            var taskToArchive = sut.ActiveTaskList[0];

            taskListDataServiceMock.Setup(x => x.ArchiveTask(taskToArchive)).Returns(ArchiveTaskResult.Ok);
            var previousUnarchivedTaskCount = sut.ActiveTaskList.Count();

            // When
            sut.ArchiveTaskItemCmd.Execute(guidToArchive);

            // Then
            Assert.IsTrue(sut.ActiveTaskList.Count == previousUnarchivedTaskCount - 1);
            taskListDataServiceMock.Verify(x => x.ArchiveTask(taskToArchive), Times.Once());
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
            var sut = MVMHelpers.SutWithNullTaskList();

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
