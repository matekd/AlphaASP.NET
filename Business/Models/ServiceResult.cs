using Domain.Models;

namespace Business.Models;

public abstract class ServiceResult<TEntity>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    public IEnumerable<TEntity>? Results { get; set; }
    public TEntity? Result { get; set; }
}

public class ClientResult : ServiceResult<Client> { }
public class JobTitleResult : ServiceResult<JobTitle> { }
public class MemberAddressResult : ServiceResult<MemberAddress> { }
public class MemberResult : ServiceResult<Member> { }
public class ProjectResult : ServiceResult<Project> { }
public class StatusResult : ServiceResult<Status> { }
public class RegisterResult : ServiceResult<string> { }
public class BoolResult : ServiceResult<bool> { }