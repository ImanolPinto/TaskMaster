using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GalaSoft.MvvmLight.Messaging;
using TaskMaster.Model;
using TaskMaster.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Moq;

namespace TaskMaster.Test.MainViewModelTests
{
    [TestFixture]
    public class OpenDialogsTests
    {
        //TODO: open the summary with the union of active, archive and blacklog lists
        [Test]
        public void a_Given_a_not_null_active_task_list_when_OpenPlayedTimesSummaryCmd_is_handled_then_OpenPlayedTimesSummaryMsg_is_sent_with_the_active_task_list()
        {
            // Given
            List<TaskItem> receivedList = null;
            Messenger.Default.Register<OpenPlayedTimesSummaryMsg>(this, x => { receivedList = x.TaskItems; });
            var sut = MVMHelpers.SutWithTaskList();

            // When
            sut.OpenPlayedTimesSummaryCmd.Execute(null);

            // Then
            Assert.IsTrue(receivedList.Count == sut.ActiveTaskList.Count);
        }

        [Test]
        public void b_Given_a_non_empty_archived_task_list_when_OpenArchivedTaskCmd_is_handled_then_OpenArchivedTasksMsg_is_sent_with_the_archived_task_list()
        {
            // Given
            ICollection<TaskItem> receivedList = null;
            Messenger.Default.Register<OpenArchivedTasksViewMsg>(this, x => { receivedList = x.TaskItems; });
            var sut = MVMHelpers.SutWithArchivedTaskList();

            // When
            sut.OpenArchivedTasksViewCmd.Execute(null);

            // Then
            Assert.IsTrue(receivedList.Count == sut.ArchivedTaskList.Count);
        }

        [Test]
        public void c_Given_an_empty_archived_task_list_then_OpenArchivedTaskCmd_cannot_be_handled()
        {
            // Given
            var sut = MVMHelpers.SutWithNullTaskLists();

            // When && Then
            Assert.IsFalse(sut.OpenArchivedTasksViewCmd.CanExecute(null));
        }
    }
}
