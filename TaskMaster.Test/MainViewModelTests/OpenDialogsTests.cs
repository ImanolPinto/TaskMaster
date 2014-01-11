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
        public void Given_a_not_null_active_task_list_when_OpenPlayedTimesSummaryCmd_is_handled_then_OpenPlayedTimesSummaryMsg_is_sent_with_the_active_task_list()
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
    }
}
