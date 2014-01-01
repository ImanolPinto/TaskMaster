using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMaster.Model;
using TaskMaster.ViewModel;

namespace TaskMaster.Test.MainViewModelTests
{
    [TestFixture]
    public class PauseCommandTests
    {
        [Test]
        public void Given_a_playing_task_that_can_be_paused_in_the_TaskItemPlayer_when_it_is_selected_then_the_Pause_command_can_be_handled()
        {
            // Given
            MainViewModel sut;
            TaskItem task;
            MVMHelpers.SutWithFirstPausableTask(out sut, out task);

            // When
            sut.SelectedTask = task;

            // Then
            Assert.IsTrue(sut.PauseTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_playing_task_that_can_be_paused_in_the_TaskItemPlayer_when_another_task_is_selected_then_the_Pause_command_can_be_handled()
        {
            // Given
            MainViewModel sut;
            TaskItem task;
            MVMHelpers.SutWithFirstPausableTask(out sut, out task);

            // When
            sut.SelectedTask = sut.UnarchivedTaskList[1];

            // Then
            Assert.IsTrue(sut.PauseTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_playing_task_that_cannot_be_paused_in_the_TaskItemPlayer_when_it_is_selected_then_the_Pause_command_cannot_be_handled()
        {
            // Given
            MainViewModel sut;
            TaskItem task;
            MVMHelpers.SutWithFirstNoPausableAndPlayingTask(out sut, out task);

            // When
            sut.SelectedTask = sut.UnarchivedTaskList[1];

            // Then
            Assert.IsFalse(sut.PauseTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_pausable_task_when_it_is_not_selected_then_it_can_be_paused()
        {
            // Given
            Mock<ITaskPlayer> taskPlayerMock = null;
            var mainViewModel = MVMHelpers.SutWithTaskListAndPlayerMock(out taskPlayerMock);
            taskPlayerMock.Setup(x => x.CanPause(mainViewModel.UnarchivedTaskList[0])).Returns(true);

            // When
            mainViewModel.SelectedTask = mainViewModel.UnarchivedTaskList[1];

            // Then
            Assert.IsTrue(mainViewModel.PauseTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_pausable_task_in_the_unarchived_task_list_and_another_task_is_selected_when_pause_command_is_received_then_pause_is_called_for_the_first_task()
        {
            // Given
            Mock<ITaskPlayer> taskPlayerMock = null;
            var mainViewModel = MVMHelpers.SutWithTaskListAndPlayerMock(out taskPlayerMock);
            mainViewModel.UnarchivedTaskList[0] = new TaskItemBuilder(Guid.NewGuid()).Build();
            taskPlayerMock.Setup(x => x.CanPause(mainViewModel.UnarchivedTaskList[0])).Returns(true);
            mainViewModel.SelectedTask = mainViewModel.UnarchivedTaskList[1];

            // When
            mainViewModel.PauseTaskItemCmd.Execute(null);

            // Then
            taskPlayerMock.Verify(x => x.Pause(mainViewModel.UnarchivedTaskList[0]), Times.Once());
        }

        [Test]
        public void Given_a_task_that_has_paused_state_in_the_unarchived_task_list_When_another_playable_task_from_the_list_is_played_then_all_the_other_tasks_are_asigned_null_state()
        {
            // Given
            Mock<ITaskPlayer> taskPlayerMock = null;
            var mainViewModel = MVMHelpers.SutWithTaskListAndPlayerMock(out taskPlayerMock);
            mainViewModel.UnarchivedTaskList[0].PlayingState = PlayingState.Paused;
            var playableTask = mainViewModel.UnarchivedTaskList[1];
            taskPlayerMock.Setup(x => x.CanPlay(playableTask)).Returns(true);

            // When
            mainViewModel.SelectedTask = playableTask;
            mainViewModel.PlayTaskItemCmd.Execute(null);

            // Then
            Assert.IsFalse(mainViewModel.UnarchivedTaskList.ToList().Any(x => x.PlayingState != null && x.Id != mainViewModel.SelectedTask.Id));
        }

        [Test]
        public void Given_a_task_list_with_no_playing_task_when_pause_command_is_received_nothing_happens()
        {
            // Given
            var mainViewModel = MVMHelpers.SutWithTaskList();

            // When
            mainViewModel.PauseTaskItemCmd.Execute(null);
        }

    }
}
