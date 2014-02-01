using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TaskMaster.Model;
using TaskMaster.ViewModel;

namespace TaskMaster.Test
{
    [TestFixture]
    public class ArchiveViewModelTests
    {
        [Test]
        public void Given_an_ArchiveViewModel_that_is_listening_to_OpenArchivedTasksViewMsg_When_the_message_is_null_then_the_archived_tasks_list_is_null()
        {
            // Given
            var sut = new ArchiveViewModel();
            sut.RegisterForMessagesBeforeView();

            // When
            Messenger.Default.Send<OpenArchivedTasksViewMsg>(null);

            // Then
            Assert.IsNull(sut.ArchiveTaskList);
        }

        [Test]
        public void Given_an_ArchiveViewModel_that_has_some_tasks_and_is_listening_to_OpenArchivedTasksViewMsg_When_the_message_contains_other_tasks_then_the_archived_tasks_list_contains_only_those_tasks()
        {
            // Given
            var sut = new ArchiveViewModel();
            sut.RegisterForMessagesBeforeView();

            sut.ArchiveTaskList = new ObservableCollection<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).Build(),
                new TaskItemBuilder(Guid.NewGuid()).Build(),
                new TaskItemBuilder(Guid.NewGuid()).Build()
            };

            var msg = new OpenArchivedTasksViewMsg(new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).WithTag("OtherTask").Build()
            });

            // When
            Messenger.Default.Send<OpenArchivedTasksViewMsg>(msg);

            // Then
            Assert.IsTrue(sut.ArchiveTaskList.Count() == 1);
            Assert.IsTrue(sut.ArchiveTaskList[0].Tag == "OtherTask");
        }
    }
}
