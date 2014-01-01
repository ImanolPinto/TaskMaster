using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TaskMaster.Model;

namespace TaskMaster.Test.MainViewModelTests
{
    [TestFixture]
    public class AddTaskTests
    {

        [Test]
        public void Given_a_non_empty_TaskList_When_the_AddTask_command_is_handled_a_new_empty_task_is_added_to_the_task_list_at_the_first_position()
        {
            // Given
            var sut = MVMHelpers.SutWithTaskList();
            var previousTaskListCount = sut.UnarchivedTaskList.Count();
            var previousTaskListFirstElement = sut.UnarchivedTaskList.First();

            // When
            sut.AddNewTaskItemCmd.Execute(null);

            // Then
            Assert.IsTrue(sut.UnarchivedTaskList.Count == previousTaskListCount + 1);
            Assert.IsTrue(sut.UnarchivedTaskList.First() != previousTaskListFirstElement);
        }

        [Test]
        public void Given_a_null_TaskList_When_the_AddTask_command_is_handled_then_the_task_list_is_null()
        {
            // Given
            var sut = MVMHelpers.SutWithNullTaskList();

            // When
            sut.AddNewTaskItemCmd.Execute(null);

            // Then
            Assert.IsTrue(sut.UnarchivedTaskList == null);
        }

        [Test]
        public void Given_an_empty_TaskList_When_the_AddTask_command_is_handled_then_the_task_list_count_is_1()
        {
            // Given
            var sut = MVMHelpers.SutWithNullTaskList();
            sut.UnarchivedTaskList = new ObservableCollection<TaskItem>();

            // When
            sut.AddNewTaskItemCmd.Execute(null);

            // Then
            Assert.IsTrue(sut.UnarchivedTaskList.Count == 1);
        }
    }
}
