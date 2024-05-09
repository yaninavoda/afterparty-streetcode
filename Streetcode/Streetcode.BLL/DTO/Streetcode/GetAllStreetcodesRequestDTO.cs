﻿namespace Streetcode.BLL.Dto.Streetcode
{
    public class GetAllStreetcodesRequestDto
    {
        public int Page { get; set; } = 1;
        public int Amount { get; set; } = 10;
        public string? Title { get; set; } = null;
        public string? Sort { get; set; } = null;
        public string? Filter { get; set; } = null;
    }
}
