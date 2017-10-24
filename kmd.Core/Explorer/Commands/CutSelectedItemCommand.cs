﻿using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;
using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.X)]
    public class CutSelectedItemCommand : ExplorerCommandBase
    {
        public CutSelectedItemCommand(ICilpboardService cilpboardService)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
        }

        protected readonly ICilpboardService _clipboardService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItem != null && vm.SelectedItem.IsPhysical;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var dataObject = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Move
            };
            dataObject.SetStorageItems(new List<IStorageItem>() { vm.SelectedItem.StorageItem });
            _clipboardService.Set(dataObject);
        }
    }
}