using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ_14
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Lesson> lessons;
        private IDisposable timerDisposable;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLessons();


            UpdateSchedule();
            StartNotificationTimer();
        }

        private void InitializeLessons()
        {

            lessons = new List<Lesson>
            {
                new Lesson { Name = "Математика", Time = new TimeSpan(8, 0, 0) },
                new Lesson { Name = "Физика", Time = new TimeSpan(9, 30, 0) },
                new Lesson { Name = "История", Time = new TimeSpan(11, 29, 0) },
                new Lesson { Name = "Английский", Time = new TimeSpan(13, 30, 0) },
                new Lesson { Name = "Биология", Time = new TimeSpan(15, 0, 0) }
            };
        }

        private void UpdateSchedule()
        {

            var currentTime = DateTime.Now.TimeOfDay;

            var nextLesson = lessons.Where(l => l.Time >= currentTime)
                                    .OrderBy(l => l.Time)
                                    .FirstOrDefault();

            if (nextLesson != null)
            {
                ScheduleLabel.Content = $"Следующее занятие: {nextLesson.Name} в {nextLesson.Time}";
            }
            else
            {
                ScheduleLabel.Content = "На сегодня занятий больше нет";
            }
        }

        private void StartNotificationTimer()
        {
            timerDisposable = Observable.Interval(TimeSpan.FromMinutes(1))
            .Subscribe(_ =>
            {

                UpdateSchedule();


                if (IsNotificationTime())
                {
                    ShowNotification();
                }
            });
        }

        private bool IsNotificationTime()
        {

            var currentTime = DateTime.Now.TimeOfDay;


            return lessons.Any(l => l.Time >= currentTime && l.Time <= currentTime.Add(TimeSpan.FromMinutes(10)));
        }

        private void ShowNotification()
        {

            MessageBox.Show("У вас скоро занятие!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Lesson
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
    }
}
}
