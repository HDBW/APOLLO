// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ImagemapViewModel : AbstractQuestionViewModel<Imagemap, ImagemapEntry>
    {
        public ImagemapViewModel(
            IAssessmentService service,
            IRawDataCacheRepository repository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ImagemapViewModel> logger)
            : base(service, repository, dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override ImagemapEntry CreateEntry(Imagemap data)
        {
            return ImagemapEntry.Import(data, MediaBasePath, Density, ImageSizeConfig[typeof(ImagemapEntry)]);
        }
    }
}
