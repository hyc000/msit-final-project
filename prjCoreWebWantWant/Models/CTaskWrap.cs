using System.ComponentModel;

namespace prjCoreWebWantWant.Models;

    public class CTaskWrap
    {
        private TaskList _task = null;
        public TaskList task
        {
            get { return _task; }
            set { _task = value; }
        }
        public CTaskWrap()
        {
            _task = new TaskList();
        }

        public int FId
        {
            get { return _task.CaseId; }
            set { _task.CaseId = value; }
        }
        public string? FTitle
        {
            get { return _task.TaskTitle; }
            set { _task.TaskTitle = value; }
        }
        public string? FDetail
        {
            get { return _task.TaskDetail; }
            set { _task.TaskDetail = value; }
        }
        public int? FPayFrom
        {
            get { return _task.PayFrom; }
            set { _task.PayFrom = value; }
        }
        public int? FPayTo
        {
            get { return _task.PayTo; }
            set { _task.PayTo = value; }
        }
        public int? FTownId
        {
            get { return _task.TownId; }
            set { _task.TownId = value; }
        }
        public string? FAddress
        {
            get { return _task.Address; }
            set { _task.Address = value; }
        }
        public string? FRequiredNum
        {
            get { return _task.RequiredNum; }
            set { _task.RequiredNum = value; }
        }
        public string? FTaskPeriod
        {
            get { return _task.TaskPeriod; }
            set { _task.TaskPeriod = value; }
        }
        //public string? FTaskStart
        //{
        //    get { return _task.TaskStart; }
        //    set { _task.TaskStart = value; }
        //}
        //public string? FTaskEnd
        //{
        //    get { return _task.TaskEnd; }
        //    set { _task.TaskEnd = value; }
        //}
        public string? FPublishOrNot
        {    
            get { return _task.PublishOrNot; }
            set { _task.PublishOrNot = value; }
        }
        public string? FPublishStart
        {
            get { return _task.PublishStart; }
            set { _task.PublishStart = value; }
        }
        public string? FPublishEnd
        {
            get { return _task.PublishEnd; }
            set { _task.PublishEnd = value; }
        }
    }

