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
    public class PlayCommandTests
    {
        [Test]
        public void Given_a_selected_task_that_can_be_played_when_the_play_command_is_executed_then_reset_and_play_is_called_once_for_that_task()
        {
            // Given
            MainViewModel mainViewModel;
            Mock<ITaskPlayer> taskPlayer;
            mainViewModel = MVMHelpers.SutWithTaskListAndPlayerMock(out taskPlayer);
            mainViewModel.SelectedTask = mainViewModel.ActiveTaskList[0];
            taskPlayer.Setup(x => x.CanPlay(mainViewModel.SelectedTask)).Returns(true);

            // When
            mainViewModel.PlayTaskItemCmd.Execute(null);

            // Then
            taskPlayer.Verify(x => x.CanPlay(mainViewModel.SelectedTask), Times.Once());
            taskPlayer.Verify(x => x.Play(mainViewModel.SelectedTask), Times.Once());
            taskPlayer.Verify(x => x.Reset(), Times.Once());
        }

        [Test]
        public void When_no_task_item_is_selected_then_no_task_can_be_played()
        {
            // Given
            var mainViewModel = MVMHelpers.SutWithNullTaskLists();
            mainViewModel.SelectedTask = null;

            // When
            mainViewModel.PlayTaskItemCmd.Execute(null);

            // Then
            Assert.IsFalse(mainViewModel.PlayTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_task_list_and_a_selected_task_that_is_not_in_the_list_then_the_selected_task_cannot_be_played()
        {
            // Given
            var mainViewModel = MVMHelpers.SutWithTaskList();
            mainViewModel.SelectedTask = new TaskItemBuilder(Guid.NewGuid()).Build();

            // Then
            Assert.IsFalse(mainViewModel.PlayTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_playing_task_item_from_the_task_list_when_it_is_selected_then_it_cannot_be_played()
        {
            // Given
            var mainViewModel = MVMHelpers.SutWithTaskList();
            mainViewModel.SelectedTask = mainViewModel.ActiveTaskList[0];
            mainViewModel.SelectedTask.PlayingState = PlayingState.Playing;

            // When
            Assert.IsFalse(mainViewModel.PlayTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void When_a_task_from_the_list_is_playing_no_task_can_be_played()
        {
            // Given
            var mainViewModel = MVMHelpers.SutWithTaskList();
            mainViewModel.SelectedTask = mainViewModel.ActiveTaskList[0];
            mainViewModel.PlayTaskItemCmd.Execute(null);

            // When
            mainViewModel.SelectedTask = mainViewModel.ActiveTaskList[1];

            // Then
            Assert.IsFalse(mainViewModel.PlayTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_task_that_can_be_played_in_the_TaskItemPlayer_when_it_is_selected_then_the_Play_command_can_be_handled()
        {
            // Given
            MainViewModel sut;
            TaskItem task;
            MVMHelpers.SutWithFirstPlayableTask(out sut, out task);

            // When
            sut.SelectedTask = task;

            // Then
            Assert.IsTrue(sut.PlayTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_task_that_can_be_played_in_the_TaskItemPlayer_when_it_is_not_selected_then_the_Play_command_cannot_be_handled()
        {
            // Given
            MainViewModel sut;
            TaskItem task;
            MVMHelpers.SutWithFirstPlayableTask(out sut, out task);

            // When
            sut.SelectedTask = null;

            // Then
            Assert.IsFalse(sut.PlayTaskItemCmd.CanExecute(null));
        }

        [Test]
        public void Given_a_task_that_cannot_be_played_in_the_TaskItemPlayer_when_it_is_selected_then_the_Play_command_cannot_be_handled()
        {
            // Given
            MainViewModel sut;
            TaskItem task;
            var isPlayable = false;
            MVMHelpers.SutWithFirstTaskPlaySetup(out sut, out task, isPlayable);

            // When
            sut.SelectedTask = task;

            // Then
            Assert.IsFalse(sut.PlayTaskItemCmd.CanExecute(null));
        }
    }
}
