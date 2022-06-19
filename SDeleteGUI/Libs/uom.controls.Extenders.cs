#nullable enable


namespace uom.controls.Extenders
{

	#region Base Classes

	internal static class Common
	{
		internal const string C_CATEGORY_COMMON = "UOM Controls Extensions";

		internal abstract class LinkedPropertiesBase(Component attachedComponent)
		{
			public Component AttachedObject = attachedComponent;

			public Control? AttachedControl => AttachedObject as Control;
		}


		internal abstract class ControlExtenderBase : System.ComponentModel.Component, IExtenderProvider, ISupportInitialize
		{

			protected Dictionary<Component, LinkedPropertiesBase> _props = [];
			private System.ComponentModel.Container? components = null;

			public ControlExtenderBase() : base() => InitializeComponent();

			public ControlExtenderBase(System.ComponentModel.IContainer container) : this() { container.Add(this); }
			protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
			private void InitializeComponent() { components = new System.ComponentModel.Container(); }

			bool IExtenderProvider.CanExtend(object o) => CanExtend((Component)o);
			protected abstract bool CanExtend(Component c);

			protected abstract LinkedPropertiesBase CreateLinkedProperties(Component c);
			protected LinkedPropertiesBase EnsurePropertiesExists(Component c)
			{
				bool found = _props.TryGetValue(c, out LinkedPropertiesBase? prop);
				if (!found)
				{
					prop = CreateLinkedProperties(c);
					_props.Add(c, prop);
				}
				return prop!;
			}


			protected virtual void OnPropertiesChanged(LinkedPropertiesBase prop) { }


			#region ISupportInitialize

			private bool _initCompleted = false;
			protected bool InitCompleted => _initCompleted;

			void ISupportInitialize.BeginInit() => OnBeginInit();
			protected virtual void OnBeginInit() { }

			void ISupportInitialize.EndInit()
			{
				OnEndInit();

				_initCompleted = true;

				if (DesignMode) return;

				foreach (LinkedPropertiesBase prop in _props.Values)
				{
					switch (prop.AttachedObject)
					{
						case Control ctl:
							OnAttachControl(ctl);
							break;

						default:
							OnAttachComponent(prop.AttachedObject);
							break;
					}
				}
			}

			protected virtual void OnEndInit() { }

			/// <summary>Works only when NOT DesignMode!</summary>			
			protected virtual void OnAttachControl(Control c) { }

			/// <summary>Works only when NOT DesignMode!</summary>			
			protected virtual void OnAttachComponent(Component c) { }


			#endregion
		}

	}

	#endregion


	/// <summary>Allows automatic open specifed URL on click</summary>
	[Description("Allows automatic open specifed URL on click")]
	[ProvideProperty("ClickAction", typeof(Component)),
		ProvideProperty("ClickActionUrl", typeof(Component))]
	internal class UrlClickHandler() : Common.ControlExtenderBase()
	{
		private const string C_CATEGORY = Common.C_CATEGORY_COMMON;
		private const string C_DEFAULT_URL = "www.sampleurl";

		public event EventHandler<Exception> OpenUrlError = delegate { };

		public enum ClickActionModes : int
		{
			None = 0,
			OpenUrlInBrowser,
			OpenFolderInExplorer,
			ShowFileInExplorer,
		}

		private class LinkedProperties(Component attachedComponent) : Common.LinkedPropertiesBase(attachedComponent)
		{
			public ClickActionModes ClickAction = ClickActionModes.None;
			public string ClickActionUrl = C_DEFAULT_URL;
		}

		public UrlClickHandler(System.ComponentModel.IContainer container) : this() { container.Add(this); }

		protected override bool CanExtend(Component c)
		{
			if (c is ToolStripStatusLabel) return true;
			if (c is LinkLabel) return true;
			return false;
		}

		protected override Common.LinkedPropertiesBase CreateLinkedProperties(Component c) => new LinkedProperties(c);

		private new LinkedProperties EnsurePropertiesExists(Component c) => (LinkedProperties)base.EnsurePropertiesExists(c);


		#region ToolStripStatusLabel

		//[Description("This string will be drawn in the center of the panel")]
		[Category(C_CATEGORY)]
		public ClickActionModes GetClickAction(Component ctl) => EnsurePropertiesExists(ctl).ClickAction;
		public void SetClickAction(Component ctl, ClickActionModes value) => EnsurePropertiesExists(ctl).ClickAction = value;
		private bool ShouldSerializeClickAction(Component ctl) => true;
		private void ResetClickAction(Component ctl) => SetClickAction(ctl, ClickActionModes.None);

		[DefaultValue(C_DEFAULT_URL)]
		[Category(C_CATEGORY)]
		[Description("Url to open in browser or Folder/ffile path to display in explorer")]
		public string GetClickActionUrl(Component ctl) => EnsurePropertiesExists(ctl).ClickActionUrl;
		public void SetClickActionUrl(Component ctl, string value) => EnsurePropertiesExists(ctl).ClickActionUrl = value;

		#endregion

		/*
		#region ToolStripStatusLabel

		//[Description("This string will be drawn in the center of the panel")]
		[Category(C_CATEGORY)]
		public ClickActionModes GetClickAction(ToolStripStatusLabel ctl) => EnsurePropertiesExists(ctl).ClickAction;
		public void SetClickAction(ToolStripStatusLabel ctl, ClickActionModes value) => EnsurePropertiesExists(ctl).ClickAction = value;
		private bool ShouldSerializeClickAction(ToolStripStatusLabel ctl) => true;
		private void ResetClickAction(ToolStripStatusLabel ctl) => SetClickAction(ctl, ClickActionModes.OpenUrlInBrowser);

		[DefaultValue(C_DEFAULT_URL)]
		[Category(C_CATEGORY)]
		[Description("Url to open in browser or Folder/ffile path to display in explorer")]
		public string GetClickActionUrl(ToolStripStatusLabel ctl) => EnsurePropertiesExists(ctl).ClickActionUrl;
		public void SetClickActionUrl(ToolStripStatusLabel ctl, string value) => EnsurePropertiesExists(ctl).ClickActionUrl = value;

		#endregion

		#region LinkLabel
	//[Description("This string will be drawn in the center of the panel")]
	[Category(C_CATEGORY)]
	public ClickActionModes GetClickAction(LinkLabel ctl) => EnsurePropertiesExists(ctl).ClickAction;
	public void SetClickAction(LinkLabel ctl, ClickActionModes value) => EnsurePropertiesExists(ctl).ClickAction = value;
	private bool ShouldSerializeClickAction(LinkLabel ctl) => true;
	private void ResetClickAction(LinkLabel ctl) => SetClickAction(ctl, ClickActionModes.OpenUrlInBrowser);

	[DefaultValue(C_DEFAULT_URL)]
	[Category(C_CATEGORY)]
	[Description("Url to open in browser or Folder/ffile path to display in explorer")]
	public string GetClickActionUrl(LinkLabel ctl) => EnsurePropertiesExists(ctl).ClickActionUrl;
	public void SetClickActionUrl(LinkLabel ctl, string value) => EnsurePropertiesExists(ctl).ClickActionUrl = value;

		#endregion

		 */


		protected override void OnAttachComponent(Component c)
		{
			/*
			// This shouldn't get called at run time, but in case it does don't do anything
			if (DesignMode)
			{
				l.Click -= new EventHandler(OnLabelClick);
				l.Click += new EventHandler(OnLabelClick);
			}									
			 */

			switch (c)
			{
				case ToolStripStatusLabel tssl:
					{
						tssl.IsLink = true;
						tssl.Click += new EventHandler(OnLabelClick!);
						break;
					}

				case LinkLabel ll:
					{
						ll.Click += new EventHandler(OnLabelClick!);
						break;
					}

				default: break;
			}
		}

		private void OnLabelClick(object sender, EventArgs e)
		{
			try
			{
				OnClick((Component)sender);
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				OpenUrlError?.Invoke(sender, ex);
			}
		}

		private void OnClick(Component c) => OpenUrl(EnsurePropertiesExists(c));
		private void OpenUrl(LinkedProperties prop)
		{
			switch (prop.ClickAction)
			{
				case ClickActionModes.None: { break; }

				case ClickActionModes.OpenUrlInBrowser:
					{
						prop.ClickActionUrl.e_OpenURLInBrowser();
						break;
					}

				case ClickActionModes.ShowFileInExplorer:
				case ClickActionModes.OpenFolderInExplorer:
					{

						var ca = (prop.ClickAction == ClickActionModes.ShowFileInExplorer)
								? uom.AppTools.WindowsExplorerPathModes.SelectItem
								: uom.AppTools.WindowsExplorerPathModes.OpenPath;

						prop.ClickActionUrl.e_OpenExplorer(ca);
						break;
					}

				default:
					throw new NotImplementedException("Invalid ClickAction!");
			}

		}




	}


	/// <summary>Displays specifed text when control is enpty</summary>
	[Description("Displays specifed text when control is enpty")]
	[ProvideProperty("EmptyText", typeof(Control))]
	internal class CueBannerManager() : Common.ControlExtenderBase()
	{
		private const string C_CATEGORY = "VistaCueBanner";

		private class LinkedProperties(Component attachedComponent) : Common.LinkedPropertiesBase(attachedComponent)
		{
			public string EmptyText = string.Empty;
		}

		public CueBannerManager(System.ComponentModel.IContainer container) : this() { container.Add(this); }


		protected override bool CanExtend(Component c)
		{
			if (c is TextBox) return true;
			return false;
		}
		protected override Common.LinkedPropertiesBase CreateLinkedProperties(Component c) => new LinkedProperties(c);
		private new LinkedProperties EnsurePropertiesExists(Component c) => (LinkedProperties)base.EnsurePropertiesExists(c);


		[Category(C_CATEGORY)]
		[Description("Text to display on empty control")]
		[DefaultValue("")]
		public string GetEmptyText(Control ctl) => EnsurePropertiesExists(ctl).EmptyText;
		public void SetEmptyText(Control ctl, string value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctl);
			p.EmptyText = value;
			OnPropertiesChanged(p);
		}

		private bool ShouldSerializeEmptyText(Control ctl) => GetEmptyText(ctl).e_IsNOTNullOrWhiteSpace();
		private void ResetEmptyText(Control ctl) => SetEmptyText(ctl, string.Empty);


		protected override void OnPropertiesChanged(Common.LinkedPropertiesBase prop) => SetCueBanner((LinkedProperties)prop);

		private void SetCueBanner(LinkedProperties prop)
		{
			switch (prop.AttachedControl!)
			{
				case TextBox tb:
					{
						tb.e_SetVistaCueBanner(prop.EmptyText);
						break;
					}

				case ListView lvw:
					{
						//lvw.se .e_SetVistaCueBanner(emptyText);
						break;
					}


				default: break;
			}
		}

	}


	[ProvideProperty("ErrorBackColor", typeof(Control))]
	[ProvideProperty("ErrorState", typeof(Control))]
	[ProvideProperty("FlashOnError", typeof(Control))]
	[ProvideProperty("ResetErrorOnFocus", typeof(Control))]
	internal class ColoredErrorProvider : Common.ControlExtenderBase
	{
		private const string C_CATEGORY = Common.C_CATEGORY_COMMON;
		private static readonly Color C_DEFAILT_ERROR_BACK_COLOR = Color.Pink;

		private const int C_DEFAULT_FLASH_TIMER_INTERVAL = 500;
		private const bool C_DEFAULT_ResetErrorOnFocus = true;
		private const bool C_DEFAULT_FlashOnError = true;



		private class LinkedProperties(Component attachedComponent) : Common.LinkedPropertiesBase(attachedComponent)
		{
			public Color ErrorBackColor = C_DEFAILT_ERROR_BACK_COLOR;
			public bool ErrorState = false;

			public bool FlashOnError = C_DEFAULT_FlashOnError;
			public bool ResetErrorOnFocus = C_DEFAULT_ResetErrorOnFocus;

			public Color ColorBeforeFlashing = SystemColors.Control;
			public bool IsFlashingStarted = false;
		}

		//private bool _initCompleted = false;
		private int _flashTimerInterval = C_DEFAULT_FLASH_TIMER_INTERVAL;
		private bool _flashState = false;
		private System.Windows.Forms.Timer _tmrFlash;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public ColoredErrorProvider() : base() => InitTimer();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public ColoredErrorProvider(System.ComponentModel.IContainer container) : this() { container.Add(this); }


		private void InitTimer()
		{
			_tmrFlash = new() { Enabled = false };
			_tmrFlash.Tick += (s, e) => DoFlash();
		}


		protected override bool CanExtend(Component c)
		{
			if (c is TextBox) return true;
			if (c is ListBox) return true;
			if (c is ComboBox) return true;
			if (c is RadioButton) return true;
			if (c is CheckBox) return true;
			if (c is Button) return true;
			if (c is NumericUpDown) return true;
			if (c is MaskedTextBox) return true;
			return false;
		}

		protected override Common.LinkedPropertiesBase CreateLinkedProperties(Component c) => new LinkedProperties(c);
		private LinkedProperties EnsurePropertiesExists(Control c) => (LinkedProperties)base.EnsurePropertiesExists(c);



		//[Description("Text to display on empty control")]		[DefaultValue("")]
		[Category(C_CATEGORY)]
		public Color GetErrorBackColor(Control ctl) => EnsurePropertiesExists(ctl).ErrorBackColor;
		public void SetErrorBackColor(Control ctl, Color value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctl);
			p.ErrorBackColor = value;
			OnPropertiesChanged(p);
		}
		private bool ShouldSerializeErrorBackColor(Control ctl) => GetErrorBackColor(ctl) != C_DEFAILT_ERROR_BACK_COLOR;
		private void ResetErrorBackColor(Control ctl) => SetErrorBackColor(ctl, C_DEFAILT_ERROR_BACK_COLOR);


		[DefaultValue(false)]
		[Category(C_CATEGORY)]
		public bool GetErrorState(Control ctl) => EnsurePropertiesExists(ctl).ErrorState;
		public void SetErrorState(Control ctl, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctl);
			p.ErrorState = value;
			OnPropertiesChanged(p);
		}
		private bool ShouldSerializeErrorState(Control ctl) => GetErrorState(ctl);
		public void ResetErrorState(Control ctl) => SetErrorState(ctl, false);



		[DefaultValue(C_DEFAULT_FlashOnError)]
		[Category(C_CATEGORY)]
		public bool GetFlashOnError(Control ctl) => EnsurePropertiesExists(ctl).FlashOnError;
		public void SetFlashOnError(Control ctl, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctl);
			p.FlashOnError = value;
			OnPropertiesChanged(p);
		}
		private bool ShouldSerializeFlashOnError(Control ctl) => GetFlashOnError(ctl) != C_DEFAULT_FlashOnError;



		[DefaultValue(C_DEFAULT_ResetErrorOnFocus)]
		[Category(C_CATEGORY)]
		public bool GetResetErrorOnFocus(Control ctl) => EnsurePropertiesExists(ctl).ResetErrorOnFocus;
		public void SetResetErrorOnFocus(Control ctl, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctl);
			p.ResetErrorOnFocus = value;
			OnPropertiesChanged(p);
		}
		private bool ShouldSerializeResetErrorOnFocus(Control ctl) => GetResetErrorOnFocus(ctl) != C_DEFAULT_ResetErrorOnFocus;



		private void DoFlash()
		{
			if (!InitCompleted) return;

			_flashState = !_flashState;
			foreach (var p in _props.Values)
			{
				LinkedProperties prop = EnsurePropertiesExists((Control)p.AttachedObject);
				DoFlash(prop);
			}
		}


		private void DoFlash(LinkedProperties prop)
		{
			if (!InitCompleted) return;

			Control ctl = (Control)prop.AttachedObject;

			Action<Color> cbSetCtlColor = (c) =>
			{
				if (c != ctl.BackColor)
				{
					ctl.BackColor = c;
					ctl.Update();
				}

			};

			if (prop.ErrorState)
			{
				if (prop.IsFlashingStarted)
				{
					Color clr = !prop.FlashOnError
						? prop.ErrorBackColor
						: _flashState
							? prop.ErrorBackColor
							: prop.ColorBeforeFlashing;

					cbSetCtlColor.Invoke(clr);
				}
				else
				{
					//Starting to flash
					prop.ColorBeforeFlashing = ctl.BackColor;
					prop.IsFlashingStarted = true;
				}
			}
			else //Control Is Not In Error
			{
				if (prop.IsFlashingStarted)
					cbSetCtlColor.Invoke(prop.ColorBeforeFlashing);//Stopping flashing and restore control BackColor

				prop.IsFlashingStarted = false;
			}
		}

		protected override void OnPropertiesChanged(Common.LinkedPropertiesBase prop) => DoFlash((LinkedProperties)prop);


		protected override void OnAttachControl(Control c)
		{
			_tmrFlash.Interval = _flashTimerInterval;
			_tmrFlash.Enabled = true;
			_tmrFlash.Start();

			c.GotFocus += (s, e) =>
			{
				Control ctl = (Control)s!;
				LinkedProperties prop = EnsurePropertiesExists(ctl);
				if (prop.ErrorState && prop.ResetErrorOnFocus) ResetErrorState(ctl);
			};
		}

	}


	[ProvideProperty("AutoCloseOnESCKey", typeof(Form))]
	internal class FormAutoCloser() : Common.ControlExtenderBase()
	{
		private const string C_CATEGORY = Common.C_CATEGORY_COMMON;

		private class LinkedProperties(Component attachedComponent) : Common.LinkedPropertiesBase(attachedComponent)
		{
			public bool AutoCloseOnESCKey = true;
		}

		public FormAutoCloser(System.ComponentModel.IContainer container) : this() { container.Add(this); }


		protected override bool CanExtend(Component c) => c is Form;

		protected override Common.LinkedPropertiesBase CreateLinkedProperties(Component c) => new LinkedProperties(c);
		private new LinkedProperties EnsurePropertiesExists(Component c) => (LinkedProperties)base.EnsurePropertiesExists(c);


		//[DefaultValue(true)]
		[Category(C_CATEGORY)]
		public bool GetAutoCloseOnESCKey(Control ctl) => EnsurePropertiesExists(ctl).AutoCloseOnESCKey;
		public void SetAutoCloseOnESCKey(Control ctl, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctl);
			p.AutoCloseOnESCKey = value;
			OnPropertiesChanged(p);
		}

		//private bool ShouldSerializeAutoCloseOnESCKey(Control ctl) => GetAutoCloseOnESCKey(ctl);
		//private void ResetAutoCloseOnESCKey(Control ctl) => SetAutoCloseOnESCKey(ctl, true);


		protected override void OnAttachControl(Control c)
		{
			Form f = (Form)c;
			f.KeyPreview = true;
			f.KeyDown += (s, e) =>
			{
				Form f2 = (Form)s!;
				LinkedProperties p = EnsurePropertiesExists(f2);

				if (!p.AutoCloseOnESCKey || e.KeyCode != Keys.Escape) return;


				if (f2.Modal)
					f2.DialogResult = DialogResult.Cancel;
				else
					f2.Close();
			};
		}

	}



	[ProvideProperty("Target", typeof(Control))]
	[ProvideProperty("InversedState", typeof(Control))]
	[ProvideProperty("TrackSourceEnableState", typeof(Control))]
	[ProvideProperty("TrackSourceEnableInverse", typeof(Control))]
	internal class CheckBoxEnabler() : Common.ControlExtenderBase()
	{
		private const string C_CATEGORY = Common.C_CATEGORY_COMMON;
		private static readonly Color C_DEFAILT_ERROR_BACK_COLOR = Color.Pink;

		private const int C_DEFAULT_FLASH_TIMER_INTERVAL = 500;
		private const bool C_DEFAULT_ResetErrorOnFocus = true;
		private const bool C_DEFAULT_FlashOnError = true;



		private class LinkedProperties(Component attachedComponent) : Common.LinkedPropertiesBase(attachedComponent)
		{
			public Control? Target = null;
			public bool InversedState = false;

			public bool TrackSourceEnableState = true;
			public bool TrackSourceEnableInverse = false;
		}


		public CheckBoxEnabler(System.ComponentModel.IContainer container) : this() { container.Add(this); }


		protected override bool CanExtend(Component c)
		{
			if (c is CheckBox) return true;
			if (c is RadioButton) return true;
			return false;
		}

		protected override Common.LinkedPropertiesBase CreateLinkedProperties(Component c) => new LinkedProperties(c);
		private LinkedProperties EnsurePropertiesExists(Control ctlCheck) => (LinkedProperties)base.EnsurePropertiesExists(ctlCheck);



		[DefaultValue(false)]
		[Category(C_CATEGORY)]
		public bool GetInversedState(Control ctlCheck) => EnsurePropertiesExists(ctlCheck).InversedState;
		public void SetInversedState(Control ctlCheck, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctlCheck);
			p.InversedState = value;
			OnPropertiesChanged(p);
		}
		//private bool ShouldSerializeInversedState(Control ctl) => GetInversedState(ctl);
		//public void ResetInversedState(Control ctl) => SetInversedState(ctl, false);


		[Category(C_CATEGORY)]
		public Control? GetTarget(Control ctlCheck) => EnsurePropertiesExists(ctlCheck).Target;
		public void SetTarget(Control ctlCheck, Control? value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctlCheck);
			//if (p.Target != null) OnAttachCheckedControl(p.Target, true);
			p.Target = value;
			//if (InitCompleted && value != null) OnAttachControl(value!);
			OnPropertiesChanged(p);
		}
		private bool ShouldSerializeTarget(Control ctl) => GetTarget(ctl) != null;


		[DefaultValue(true)]
		[Category(C_CATEGORY)]
		public bool GetTrackSourceEnableState(Control ctlCheck) => EnsurePropertiesExists(ctlCheck).TrackSourceEnableState;
		public void SetTrackSourceEnableState(Control ctlCheck, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctlCheck);
			p.TrackSourceEnableState = value;
			OnPropertiesChanged(p);
		}
		//private bool ShouldSerializeTrackSourceEnableState(Control ctl) => GetTrackSourceEnableState(ctl);
		//public void ResetInversedState(Control ctl) => SetInversedState(ctl, false);


		[DefaultValue(false)]
		[Category(C_CATEGORY)]
		public bool GetTrackSourceEnableInverse(Control ctlCheck) => EnsurePropertiesExists(ctlCheck).TrackSourceEnableInverse;
		public void SetTrackSourceEnableInverse(Control ctlCheck, bool value)
		{
			LinkedProperties p = EnsurePropertiesExists(ctlCheck);
			p.TrackSourceEnableInverse = value;
			OnPropertiesChanged(p);
		}
		//private bool ShouldSerializeTrackSourceEnableState(Control ctl) => GetTrackSourceEnableState(ctl);
		//public void ResetInversedState(Control ctl) => SetInversedState(ctl, false);





		protected override void OnPropertiesChanged(Common.LinkedPropertiesBase prop)
		{
			if (prop.AttachedControl != null) OnSource_PropChanged(prop.AttachedControl);
		}


		protected override void OnAttachControl(Control ctlCheck)
		{
			switch (ctlCheck)
			{
				case CheckBox chk:
					chk.CheckedChanged += OnSource_CheckedChanged!;
					break;

				case RadioButton opt:
					opt.CheckedChanged += OnSource_CheckedChanged!;
					break;

				default: throw new NotImplementedException($"Unknown control type ('{ctlCheck.GetType()}') to attach to evens!");
			}
			ctlCheck.EnabledChanged += OnSource_EnabledChanged!;
			OnSource_PropChanged(ctlCheck);
		}


		private void OnSource_EnabledChanged(object s, EventArgs e) => OnSource_PropChanged((Control)s);

		private void OnSource_CheckedChanged(object s, EventArgs e) => OnSource_PropChanged((Control)s);


		private void OnSource_PropChanged(Control ctlSourceCheck)
		{
			LinkedProperties prop = EnsurePropertiesExists(ctlSourceCheck);
			if (prop.Target == null) return;

			bool sourceEnabled = ctlSourceCheck.Enabled;
			bool sourceChecked = false;
			switch (ctlSourceCheck)
			{
				case CheckBox chk: sourceChecked = chk.Checked; break;
				case RadioButton opt: sourceChecked = opt.Checked; break;
				default: return;
			}

			//if (prop.TrackSourceEnableInverse) sourceEnabled = !sourceEnabled;
			if (prop.TrackSourceEnableState && !sourceEnabled)
			{
				sourceChecked = false;
			}

			if (prop.InversedState) sourceChecked = !sourceChecked;
			bool targetEnabledState = sourceChecked;

			Control ctlTarget = prop.Target!;
			if (ctlTarget.Enabled != targetEnabledState)
			{
				ctlTarget.Enabled = targetEnabledState;
				ctlTarget.Refresh();
			}
		}
	}



}
