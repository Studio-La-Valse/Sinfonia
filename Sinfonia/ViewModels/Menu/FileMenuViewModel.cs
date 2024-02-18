﻿using Sinfonia.ViewModels.Application;

namespace Sinfonia.ViewModels.Menu
{
    public class FileMenuViewModel : MenuItemViewModel
    {
        private readonly DocumentCollectionViewModel documentCollection;
        private readonly IShellMethods shellMethods;

        public FileMenuViewModel(DocumentCollectionViewModel documentCollection, ImportMenuViewModel importMenuViewModel, IShellMethods shellMethods, ICommandFactory commandFactory) : base("File")
        {
            this.documentCollection = documentCollection;
            this.shellMethods = shellMethods;

            MenuItems.Add(importMenuViewModel);
            MenuItems.Add(new MenuItemViewModel("Close", commandFactory.Create(CloseDocument, documentCollection.Documents.Any)));
            MenuItems.Add(new MenuItemViewModel("Exit", commandFactory.Create(Exit)));
        }

        public void CloseDocument()
        {
            var openDocument = documentCollection.Documents.FirstOrDefault(d => d.IsActive);
            if (openDocument is null)
            {
                return;
            }

            documentCollection.Close(openDocument);
        }

        public void Exit()
        {
            for (int i = documentCollection.Documents.Count - 1; i >= 0; i--)
            {
                var document = documentCollection.Documents[i];
                documentCollection.Close(document);
            }

            shellMethods.Exit();
        }
    }
}
