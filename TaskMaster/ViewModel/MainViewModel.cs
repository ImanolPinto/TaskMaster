using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TaskMaster.Model;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace TaskMaster.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Services

        private readonly ITaskListDataService _taskListService;
        private readonly ITimeProvider _timeProvider;
        
        #endregion

        #region Commands

        public RelayCommand PlayTaskItemCmd { get; private set; }
        public RelayCommand PauseTaskItemCmd { get; private set; }
        public RelayCommand AddNewTaskItemCmd { get; private set; }
        public RelayCommand<Guid> ArchiveTaskItemCmd { get; private set; }

        public RelayCommand OpenPlayedTimesSummaryCmd { get; private set; }
        public RelayCommand OpenArchivedTasksViewCmd { get; private set; }

        #endregion

        #region Properties

        ObservableCollection<TaskItem> _activeTaskList;
        public ObservableCollection<TaskItem> ActiveTaskList
        {
            get
            {
                return _activeTaskList;
            }

            set
            {
                if (_activeTaskList == value)
                {
                    return;
                }

                _activeTaskList = value;
                RaisePropertyChanged("ActiveTaskList");
            }
        }

        ObservableCollection<TaskItem> _archivedTaskList;
        public ObservableCollection<TaskItem> ArchivedTaskList
        {
            get
            {
                return _archivedTaskList;
            }
            set
            {
                if (_archivedTaskList == value)
                    return;

                _archivedTaskList = value;
            }
        }

        ObservableCollection<string> _availableTagList;
        public ObservableCollection<string> AvailableTagList
        {
            get
            {
                return _availableTagList;
            }

            set
            {
                if (_availableTagList == value)
                {
                    return;
                }

                _availableTagList = value;
                RaisePropertyChanged("AvailableTagList");
            }
        }

        private TaskItem _selectedTask;
        public TaskItem SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                if (value != _selectedTask)
                {
                    _selectedTask = value;
                    RaisePropertyChanged("SelectedTask");
                    UpdateAvailableTagList();
                }
            }
        }

        private ITaskPlayer _taskPlayer;
        public ITaskPlayer TaskPlayer
        {
            get
            {
                return _taskPlayer;
            }
            set
            {
                if (value != _taskPlayer)
                {
                    _taskPlayer = value;
                    RaisePropertyChanged("TaskPlayer");
                }
            }
        }

        private string _selectedTag;
        public string SelectedTag
        {
            get
            {
                return _selectedTag;
            }
            set
            {
                if (value != _selectedTag)
                {
                    _selectedTag = value;
                    SelectedTask.Tag = _selectedTag;
                    RaisePropertyChanged("SelectedTag");
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ITaskListDataService taskListService, ITaskPlayer taskPlayer, ITimeProvider timeProvider)
        {
            Ensure.IsNotNull("ITaskListDataService", taskListService);
            Ensure.IsNotNull("ITaskPlayer", taskPlayer);
            Ensure.IsNotNull("ITimeProvider", timeProvider);

            _taskListService = taskListService;
            _taskPlayer = taskPlayer;
            _timeProvider = timeProvider;

            PlayTaskItemCmd = new RelayCommand(PlayTaskItem, CanPlayTaskItem);
            PauseTaskItemCmd = new RelayCommand(PauseTaskItem, CanPauseTaskItem);
            AddNewTaskItemCmd = new RelayCommand(AddNewTaskItem, CanAddNewTaskItem);
            ArchiveTaskItemCmd = new RelayCommand<Guid>(ArchiveTaskItem, CanArchiveTaskItem);

            OpenPlayedTimesSummaryCmd = new RelayCommand(OpenPlayedTimesSummary);
            OpenArchivedTasksViewCmd = new RelayCommand(OpenArchivedTasksView, CanOpenArchivedTaskView);

            var bgWorkerActiveTasks = new BgWorkerBuilder(PopulateActiveTasks).Build();
            bgWorkerActiveTasks.RunWorkerAsync();

            var bgWorkerArchivedTasks = new BgWorkerBuilder(PopulateArchivedTasks).Build();
            bgWorkerArchivedTasks.RunWorkerAsync();

        }

        private void PopulateActiveTasks(object sender, DoWorkEventArgs e)
        {
            var taskList = _taskListService.GetActiveTasks();
            if (taskList != null)
                ActiveTaskList = new ObservableCollection<TaskItem>(taskList);

            if (IsInDesignMode)
            {
                SelectedTask = ActiveTaskList[0];
            }
        }

        private void PopulateArchivedTasks(object sender, DoWorkEventArgs e)
        {
            ArchivedTaskList = new ObservableCollection<TaskItem>(_taskListService.GetRecentArchivedTasks());
        }

        private bool CanPlayTaskItem()
        {
            return ActiveTaskList != null && 
                ActiveTaskList.Contains(_selectedTask) && 
                ThereIsNotAPausableTaskInTheList() &&
                _taskPlayer.CanPlay(_selectedTask);
        }

        private void PlayTaskItem()
        {
            ClearAllTheStatesExceptForTheTask(_selectedTask);
            _taskPlayer.Reset();
            _taskPlayer.Play(_selectedTask);
        }

        private bool CanPauseTaskItem()
        {
            var playingTask = FirstPausableTask();

            if (playingTask == null)
                return false;

            return _taskPlayer.CanPause(playingTask);
        }

        private void PauseTaskItem()
        {
            if (!CanPauseTaskItem())
                return;

            TaskItem playingTask = FirstPausableTask();
            _taskPlayer.Pause(playingTask);
            ClearAllTheStatesExceptForTheTask(playingTask);
        }

        private void AddNewTaskItem()
        {
            if (!CanAddNewTaskItem())
                return;

            var newTask = new TaskItemBuilder(Guid.NewGuid()).Build();
            ActiveTaskList.Insert(0, newTask);
        }

        private bool CanAddNewTaskItem()
        {
            return ActiveTaskList != null;
        }

        private void ArchiveTaskItem(Guid taskId)
        {
            if (!CanArchiveTaskItem(taskId))
                return;

            var taskToArchive = ActiveTaskList.FirstOrDefault(x => x.Id == taskId);
            var result = _taskListService.ArchiveTask(taskToArchive);
            
            if (result != ArchiveTaskResult.Ok)
                return;

            if (taskToArchive.PlayingState == PlayingState.Playing)
                _taskPlayer.Pause(taskToArchive);

            ActiveTaskList.Remove(taskToArchive);

            if (ArchivedTaskList == null)
                ArchivedTaskList = new ObservableCollection<TaskItem>();

            ArchivedTaskList.Insert(0, taskToArchive);
        }

        private bool CanArchiveTaskItem(Guid taskId)
        {
            return ActiveTaskList != null && ActiveTaskList.FirstOrDefault(x => x.Id == taskId) != null;
        }

        private void OpenPlayedTimesSummary()
        {
            var listToSummarize = _activeTaskList.ToList();
            Messenger.Default.Send<OpenPlayedTimesSummaryMsg>(new OpenPlayedTimesSummaryMsg(listToSummarize));
        }

        private void OpenArchivedTasksView()
        {
            Messenger.Default.Send<OpenArchivedTasksViewMsg>(new OpenArchivedTasksViewMsg(_archivedTaskList));
        }

        private bool CanOpenArchivedTaskView()
        {
            return ArchivedTaskList != null;
        }

        private void ClearAllTheStatesExceptForTheTask(TaskItem task)
        {
            this.ActiveTaskList
                .Where(x => x != task).ToList()
                .ForEach(x => x.PlayingState = null);
        }

        private bool ThereIsNotAPausableTaskInTheList()
        {
            var pausableTask = FirstPausableTask();

            if (pausableTask == null)
                return true;

            return false;
        }

        private TaskItem FirstPausableTask()
        {
            return ActiveTaskList.Where(x => _taskPlayer.CanPause(x)).FirstOrDefault() as TaskItem;
        }

        private void UpdateAvailableTagList()
        {
            AvailableTagList = new ObservableCollection<string>(_activeTaskList.Where(x => x.Tag != null && x.Tag.Trim() != String.Empty).Select(x => x.Tag).Distinct());
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}