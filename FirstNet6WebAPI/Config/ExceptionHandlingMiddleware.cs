using System.Net;
using System.Text.Json;

namespace FirstNet6WebAPI;
/// <summary>
/// 自定义全局异常处理中间件
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;  // 用来处理上下文请求  
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext); //要么在中间件中处理，要么被传递到下一个中间件中去
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex); // 捕获异常了 在HandleExceptionAsync中处理
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";  // 返回json 类型
        var response = context.Response;

        var errorResponse = new ErrorResponse();  // 自定义的异常错误信息类型
        switch (exception)
        {
            case ApplicationException ex:
                if (ex.Message.Contains("Invalid token"))
                {
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    errorResponse.Msg = ex.Message;
                    break;
                }
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                errorResponse.Msg = ex.Message;
                break;
            case KeyNotFoundException ex:
                response.StatusCode = (int) HttpStatusCode.NotFound;
                errorResponse.Msg = ex.Message;
                break;
            default:
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorResponse.Msg = "Internal Server errors. Check Logs!";
                //errorResponse.Msg = exception.Message;
                break;
        }
        errorResponse.Code = response.StatusCode;
        Common.Helper.LogUtil.Error(exception.Message);
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}

/// <summary>
/// 异常处理返回结果
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// 异常信息
    /// </summary>
    public string Msg { get; set; }
}