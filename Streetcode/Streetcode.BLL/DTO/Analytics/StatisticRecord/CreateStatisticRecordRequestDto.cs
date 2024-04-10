namespace Streetcode.BLL.DTO.Analytics.StatisticRecord;

public sealed record CreateStatisticRecordRequestDto(int QrId, int StreetcodeId, int StreetcodeCoordinateId, int Count, string Address);
