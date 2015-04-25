﻿namespace tomenglertde.ResXManager.Model
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Threading;

    using TomsToolbox.Desktop;

    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Works fine with this")]
    public class Configuration : ConfigurationBase
    {
        private readonly DispatcherThrottle _codeReferencesChangeThrottle;
        private CodeReferenceConfiguration _codeReferences;

        public Configuration()
        {
            _codeReferencesChangeThrottle = new DispatcherThrottle(DispatcherPriority.ContextIdle, PersistCodeReferences);
        }

        public CodeReferenceConfiguration CodeReferences
        {
            get
            {
                Contract.Ensures(Contract.Result<CodeReferenceConfiguration>() != null);

                return _codeReferences ?? CreateCodeReferenceConfiguration();
            }
        }

        public bool SortFileContentOnSave
        {
            get
            {
                return GetValue(() => SortFileContentOnSave);
            }
            set
            {
                SetValue(value, () => SortFileContentOnSave);
            }
        }

        public CultureInfo NeutralResourcesLanguage
        {
            get
            {
                Contract.Ensures(Contract.Result<CultureInfo>() != null);

                return GetValue(() => NeutralResourcesLanguage) ?? new CultureInfo("en-US");
            }
            set
            {
                SetValue(value, () => NeutralResourcesLanguage);
            }
        }

        public StringComparison ResXSortingComparison
        {
            get
            {
                return GetValue(() => ResXSortingComparison, StringComparison.OrdinalIgnoreCase);
            }
            set
            {
                SetValue(value, () => ResXSortingComparison);
            }
        }

        public bool ConfirmAddLanguage
        {
            get
            {
                return GetValue(() => ConfirmAddLanguage, true);
            }
            set
            {
                SetValue(value, () => ConfirmAddLanguage);
            }
        }

        public bool AutoCreateNewLanguageFiles
        {
            get
            {
                return GetValue(() => AutoCreateNewLanguageFiles, false);
            }
            set
            {
                SetValue(value, () => AutoCreateNewLanguageFiles);
            }
        }

        public bool PrefixTranslations
        {
            get
            {
                return GetValue(() => PrefixTranslations, false);
            }
            set
            {
                SetValue(value, () => PrefixTranslations);
            }
        }

        public string TranslationPrefix
        {
            get
            {
                return GetValue(() => TranslationPrefix, "#TODO#_");
            }
            set
            {
                SetValue(value, () => TranslationPrefix);
            }
        }

        private void PersistCodeReferences()
        {
            SetValue(CodeReferences, () => CodeReferences);
        }

        private CodeReferenceConfiguration CreateCodeReferenceConfiguration()
        {
            _codeReferences = GetValue(() => CodeReferences) ?? CodeReferenceConfiguration.Default;
            _codeReferences.ItemPropertyChanged += (_, __) => _codeReferencesChangeThrottle.Tick();

            return _codeReferences;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(_codeReferencesChangeThrottle != null);
        }
    }
}