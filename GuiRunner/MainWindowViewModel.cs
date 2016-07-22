using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CyclicReferenceFinder.Model;
using CyclicReferenceFinder.Parser;
using CyclicReferenceFinder.ReferencesAnalyzer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace GuiRunner
{
	public class MainWindowViewModel:ViewModelBase
	{
		private string _solutionFile;
		private bool _isBusy;
		private ObservableCollection<Project> _projects;
		private bool _isSolutionLoaded;
		private Project _sourceProject;
		private Project _referencedProject;
		private bool? _cyclicReferenceFound;
		private ObservableCollection<Chain> _chains;


		public String SolutionFile	
		{
			get { return _solutionFile; }
			set
			{
				_solutionFile = value;
				RaisePropertyChanged();
			}
		}

		public Project SourceProject
		{
			get { return _sourceProject; }
			set
			{
				_sourceProject = value;
				RaisePropertyChanged();
				FindCyclicReferenceCommand.RaiseCanExecuteChanged();
			}
		}

		public Project ReferencedProject
		{
			get { return _referencedProject; }
			set
			{
				_referencedProject = value;
				RaisePropertyChanged();
				FindCyclicReferenceCommand.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<Chain> Chains
		{
			get { return _chains; }
			set { _chains = value; RaisePropertyChanged();}
		}

		public bool? CyclicReferenceFound
		{
			get { return _cyclicReferenceFound; }
			set
			{
				_cyclicReferenceFound = value; 
				RaisePropertyChanged();
			}
		}

		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				RaisePropertyChanged();
				FindCyclicReferenceCommand.RaiseCanExecuteChanged();
				SelectSolutionCommand.RaiseCanExecuteChanged();

			}
		}

		public bool IsSolutionLoaded
		{
			get { return _isSolutionLoaded; }
			set { _isSolutionLoaded = value;RaisePropertyChanged();
				FindCyclicReferenceCommand.RaiseCanExecuteChanged();
			}
		}

		public RelayCommand SelectSolutionCommand { get; }
		public RelayCommand FindCyclicReferenceCommand { get; }

		public ObservableCollection<Project> Projects
		{
			get { return _projects; }
			set
			{
				_projects = value;
				RaisePropertyChanged();
			}
		}

		public MainWindowViewModel()
		{
			CyclicReferenceFound = true;
			SelectSolutionCommand = new RelayCommand(HandleSelectSolutionCommand, () => !IsBusy);
			FindCyclicReferenceCommand = new RelayCommand(HandleCyclicReferenceCommand, ()=>!IsBusy && IsSolutionLoaded &&SourceProject!=null && ReferencedProject!=null);
		}

		private async void HandleCyclicReferenceCommand()
		{
			IsBusy = true;
			Chains = new ObservableCollection<Chain>(await CyclicFinder.Instance.FindCylicChains(SourceProject, ReferencedProject));
			CyclicReferenceFound = Chains.Any();
			IsBusy = false;
		}

		private void HandleSelectSolutionCommand()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				CheckFileExists = true,
				Filter = "Visual studio solution file(*.sln)|*.sln",
				FileName = SolutionFile
			};

			if (openFileDialog.ShowDialog(Application.Current.MainWindow)==true)
			{
				Projects= new ObservableCollection<Project>(Parser.Instance.Parse(openFileDialog.FileName).OrderBy(p=>p.Name));
				SolutionFile = openFileDialog.FileName;
			}
			IsSolutionLoaded = true;
		}
	}
}
