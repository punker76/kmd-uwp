﻿using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Extensions;
using kmd.Core.Hotkeys;
using System;
using System.Linq;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("ChangeExtensionCommand", "ChangeExtensionCommand", key: VirtualKey.E, modifierKey: ModifierKeys.Control)]
    public class ChangeExtensionCommand : ExplorerCommandBase
    {
        public ChangeExtensionCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItem.IsFile && vm.SelectedItems.Count == 1 && vm.SelectedItem.IsPhysical;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var result = await _dialogService.Prompt("Change file extension", vm.SelectedItem.Name.Split('.').Last());

            if (result == null) return;

            try
            {
                await vm.SelectedItem.StorageItem.RenameAsync(vm.SelectedItem.DisplayName + "." + result);
                var storageItem = vm.SelectedItem.StorageItem;
                var newItem = await ExplorerItem.CreateAsync(storageItem);

                vm.ExplorerItems.Remove(vm.SelectedItem);
                vm.ExplorerItems.Add(newItem);
                vm.SelectedItem = newItem;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message, "Invalid operation", "Ok", null);
            }
        }
    }
}
