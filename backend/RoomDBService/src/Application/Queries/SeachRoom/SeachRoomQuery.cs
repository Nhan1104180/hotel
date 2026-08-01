using Application.DTO;
using Azure;
using MediatR;
using Share.CommonModel;

namespace Application.Queries.SeachRoom;

public class SeachRoomQuery : IRequest<ResponseEntity>
{
    public string Keyword { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}