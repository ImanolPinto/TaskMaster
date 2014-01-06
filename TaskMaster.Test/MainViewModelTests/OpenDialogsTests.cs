using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GalaSoft.MvvmLight.Messaging;
using TaskMaster.Model;
using TaskMaster.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace TaskMaster.Test.MainViewModelTests
{
    [TestFixture]
    public class OpenDialogsTests
    {


        //TODO: open the summary with the union of active, archive and blacklog lists
        [Test]
        public void Given_a_not_null_active_task_list_when_OpenPlayedTimesSummaryCmd_is_handled_then_OpenPlayedTimesSummaryMsg_is_sent_and_the_PlayedTimesSummaryModel_has_the_correct_play_list()
        {
            // Given
            var sut = MVMHelpers.SutWithTaskList();
            List<TaskItem> receivedList = null;
            Messenger.Default.Register<OpenPlayedTimesSummaryMsg>(this, x => { receivedList = x.TaskItems; });

            var playedTimesSummaryModel = new PlayedTimesSummaryModel();
            playedTimesSummaryModel.RegisterForMessagesBeforeView();

            // When
            sut.OpenPlayedTimesSummaryCmd.Execute(null);

            // Then
            Assert.IsTrue(receivedList.Count == sut.ActiveTaskList.Count);
            Assert.IsTrue(playedTimesSummaryModel.TaskList.Count == sut.ActiveTaskList.Count);
        }
    }
}
