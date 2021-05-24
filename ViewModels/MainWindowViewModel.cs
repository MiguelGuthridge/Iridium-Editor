using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IridiumEditor;
using ReactiveUI;
using IridiumEditor.Views;

namespace IridiumEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string windowTitle = "";
        public string WindowTitle
        {
            get => windowTitle;
            set => this.RaiseAndSetIfChanged(ref windowTitle, value);
        }
        public string Hint => "Hint Message";
        public string NoteCount => "Note Count: 17048";
        public string CursorPosition => "13.3.120 : 12";

        private readonly int project;
        private readonly MainWindow window;

        public MainWindowViewModel(MainWindow win)
        {
            project = App.Projects.CreateProject();
            window = win;
            WindowTitle = GenWindowTitle();
            
            ShowDetails = new Interaction<ProjectDetailsViewModel, Unit>();
            
            ProjectDetailsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var store = new ProjectDetailsViewModel(project);

                await ShowDetails.Handle(store);
            });
        }

        public Interaction<ProjectDetailsViewModel, Unit> ShowDetails { get; }

        public ICommand ProjectDetailsCommand { get; }
        
        private string GenWindowTitle()
        {
            return App.Projects.GetProject(project).details.Name + " - Iridium";
        }

        public void OnTitleMenuClick()
        {
            ProjectDetailsWindow detailsWindow = new(project);
            detailsWindow.ShowDialog(window);
            WindowTitle = GenWindowTitle();
        }
    }
}
