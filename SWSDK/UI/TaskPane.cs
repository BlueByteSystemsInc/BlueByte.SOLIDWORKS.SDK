using BlueByte.SOLIDWORKS.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.UI
{
    public static class TaskPane
    {
        /// <summary>
        /// Builds the task pane.
        /// </summary>
        /// <param name="TaskPaneProgID">The task pane prog identifier.</param>
        /// <param name="taskPaneTitle">The task pane title.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">taskPaneTitle</exception>
        public static ITaskPane BuildTaskPane(string TaskPaneProgID, string taskPaneTitle = "")
        {

            if (string.IsNullOrWhiteSpace(taskPaneTitle))
                throw new ArgumentNullException(nameof(taskPaneTitle));

            var solidworksApplication = AddInBase.Container.GetInstance<ISOLIDWORKSApplication>();
            var SOLIDWORKS = solidworksApplication.As<SolidWorks.Interop.sldworks.SldWorks>();

            // create taskpane
            var taskPaneView = SOLIDWORKS.CreateTaskpaneView2(string.Empty, taskPaneTitle);
            var taskPane = (ITaskPane)taskPaneView.AddControl(TaskPaneProgID, "");
            // set back color to transparent   
            return taskPane;
        }
    }


    public interface ITaskPane 
    {
         
    }

}
