using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venn.Base.Interfaces;

namespace Venn.Extensions
{
    public static  class ISuspendableNotifyPropertyChangedExtensions
    {
        // Method to suspend notifications for objects implementing ISuspendableNotifyPropertyChanged
        public static void SuspendNotifications(this ISuspendableNotifyPropertyChanged source)
        {
            // Check for null parameter
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Set the IsSuspended property to true to suspend notifications
            source.Pause();
        }

        // Method to resume notifications for objects implementing ISuspendableNotifyPropertyChanged
        public static void ResumeNotifications(this ISuspendableNotifyPropertyChanged source)
        {
            // Check for null parameter
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Set the IsSuspended property to false to resume notifications
            source.Resume();
        }
    }
}
