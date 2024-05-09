using System.Transactions;
using Streetcode.BLL.RepositoryInterfaces.AdditionalContent;
using Streetcode.BLL.RepositoryInterfaces.Analytics;
using Streetcode.BLL.RepositoryInterfaces.Media;
using Streetcode.BLL.RepositoryInterfaces.Media.Images;
using Streetcode.BLL.RepositoryInterfaces.Newss;
using Streetcode.BLL.RepositoryInterfaces.Partners;
using Streetcode.BLL.RepositoryInterfaces.Source;
using Streetcode.BLL.RepositoryInterfaces.Streetcode;
using Streetcode.BLL.RepositoryInterfaces.Streetcode.TextContent;
using Streetcode.BLL.RepositoryInterfaces.Team;
using Streetcode.BLL.RepositoryInterfaces.Timeline;
using Streetcode.BLL.RepositoryInterfaces.Toponyms;
using Streetcode.BLL.RepositoryInterfaces.Transactions;

namespace Streetcode.BLL.RepositoryInterfaces.Base
{
    public interface IRepositoryWrapper
    {
        IFactRepository FactRepository { get; }
        IArtRepository ArtRepository { get; }
        IStreetcodeArtRepository StreetcodeArtRepository { get; }
        IVideoRepository VideoRepository { get; }
        IImageRepository ImageRepository { get; }
        IImageDetailsRepository ImageDetailsRepository { get; }
        IAudioRepository AudioRepository { get; }
        IStreetcodeCoordinateRepository StreetcodeCoordinateRepository { get; }
        IPartnersRepository PartnersRepository { get; }
        ISourceCategoryRepository SourceCategoryRepository { get; }
        IStreetcodeCategoryContentRepository StreetcodeCategoryContentRepository { get; }
        IRelatedFigureRepository RelatedFigureRepository { get; }
        IStreetcodeRepository StreetcodeRepository { get; }
        ISubtitleRepository SubtitleRepository { get; }
        IStatisticRecordRepository StatisticRecordRepository { get; }
        ITagRepository TagRepository { get; }
        ITeamRepository TeamRepository { get; }
        ITeamPositionRepository TeamPositionRepository { get; }
        ITeamLinkRepository TeamLinkRepository { get; }
        ITermRepository TermRepository { get; }
        IRelatedTermRepository RelatedTermRepository { get; }
        ITextRepository TextRepository { get; }
        ITimelineRepository TimelineRepository { get; }
        IToponymRepository ToponymRepository { get; }
        ITransactLinksRepository TransactLinksRepository { get; }
        IHistoricalContextRepository HistoricalContextRepository { get; }
        IPartnerSourceLinkRepository PartnerSourceLinkRepository { get; }
        IStreetcodeTagIndexRepository StreetcodeTagIndexRepository { get; }
        IPartnerStreetcodeRepository PartnerStreetcodeRepository { get;  }
        INewsRepository NewsRepository { get; }
        IPositionRepository PositionRepository { get; }
        IHistoricalContextTimelineRepository HistoricalContextTimelineRepository { get; }
        IStreetcodeToponymRepository StreetcodeToponymRepository { get; }
        IStreetcodeImageRepository StreetcodeImageRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        public Task<int> SaveChangesAsync();

        public TransactionScope BeginTransaction();
    }
}
