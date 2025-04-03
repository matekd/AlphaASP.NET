using Domain.Models;

namespace Business.Models;

public abstract class ServiceResult<TEntity>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    public IEnumerable<TEntity>? Result { get; set; }
    public TEntity? Entity { get; set; }
}

public class ClientResult : ServiceResult<Client> { }
public class JobTitleResult : ServiceResult<JobTitle> { }
public class MemberAddressResult : ServiceResult<MemberAddress> { }
public class MemberResult : ServiceResult<Member> { }
public class ProjectResult : ServiceResult<Project> { }
public class StatusResult : ServiceResult<Status> { }