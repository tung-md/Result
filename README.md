[![vi](https://img.shields.io/badge/lang-vi-blue.svg)](https://github.com/tung-md/result/blob/master/README.md)
[![en](https://img.shields.io/badge/lang-en-green.svg)](https://github.com/tung-md/result/blob/master/README.en.md)

[![Tung.Result - NuGet](https://img.shields.io/nuget/v/Tung.Result.svg?label=Tung.Result%20-%20nuget)](https://www.nuget.org/packages/Tung.Result) [![NuGet](https://img.shields.io/nuget/dt/Tung.Result.svg)](https://www.nuget.org/packages/Tung.Result) [![Build Status](https://github.com/tung-md/Result/workflows/.NET%20Core/badge.svg)](https://github.com/tung-md/Result/actions?query=workflow%3A%22.NET+Core%22)

[![Tung.Result.AspNetCore - NuGet](https://img.shields.io/nuget/v/Tung.Result.AspNetCore.svg?label=Tung.Result.AspNetCore%20-%20nuget)](https://www.nuget.org/packages/Tung.Result.AspNetCore) [![NuGet](https://img.shields.io/nuget/dt/Tung.Result.AspNetCore.svg)](https://www.nuget.org/packages/Tung.Result.AspNetCore) &nbsp; [![Tung.Result.FluentValidation - NuGet](https://img.shields.io/nuget/v/Tung.Result.FluentValidation.svg?label=Tung.Result.FluentValidation%20-%20nuget)](https://www.nuget.org/packages/Tung.Result.FluentValidation) [![NuGet](https://img.shields.io/nuget/dt/Tung.Result.FluentValidation.svg)](https://www.nuget.org/packages/Tung.Result.FluentValidation)

# Kết quả

Bản tóm tắt kết quả có thể được ánh xạ tới mã phản hồi HTTP nếu cần.

## Dự án này giải quyết vấn đề gì??

Nhiều phương thức trên dịch vụ cần trả về một số loại giá trị. Chẳng hạn, họ có thể tra cứu một số dữ liệu và trả về một tập hợp kết quả hoặc một đối tượng. Họ có thể đang tạo ra thứ gì đó, duy trì nó và sau đó trả lại nó. Thông thường, các phương pháp như vậy được thực hiện như thế này:

```csharp
public Customer GetCustomer(int customerId)
{
  // more logic
  return customer;
}

public Customer CreateCustomer(string firstName, string lastName)
{
  // more logic
  return customer;
}
```

Điều này có hiệu quả miễn là chúng ta chỉ quan tâm đến "happy path". Nhưng điều gì sẽ xảy ra nếu có nhiều chế độ lỗi mà không phải tất cả đều có thể được xử lý bằng ngoại lệ?

- Điều gì xảy ra nếu không tìm thấy `customerId`?
- Điều gì xảy ra nếu yêu cầu `lastName` không được cung cấp?
- Điều gì xảy ra nếu người dùng hiện tại không có quyền tạo khách hàng mới?

Cách tiêu chuẩn để giải quyết những mối lo ngại này là áp dụng các trường hợp ngoại lệ. Có thể bạn đưa ra một ngoại lệ khác nhau cho từng chế độ lỗi khác nhau và khi đó mã gọi bắt buộc phải có nhiều khối được thiết kế cho từng loại lỗi. Điều này khiến cách viết mã trở nên khó khăn và dẫn đến nhiều trường hợp ngoại lệ cho những thứ không nhất thiết phải là `ngoại lệ` . Như thế này:

```csharp
[HttpGet]
public async Task<ActionResult<CustomerDTO>> GetCustomer(int customerId)
{
  try
  {
    var customer = _repository.GetById(customerId);
    
    var customerDTO = CustomerDTO.MapFrom(customer);
    
    return Ok(customerDTO);
  }
  catch (NullReferenceException ex)
  {
    return NotFound();
  }
  catch (Exception ex)
  {
    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
  }
}
```

Một cách tiếp cận khác là trả về `Tuple (Bộ dữ liệu)` kết quả mong đợi cùng với những thứ khác, như mã trạng thái và siêu dữ liệu chế độ lỗi bổ sung. Mặc dù các bộ dữ liệu có thể rất tốt cho các phản hồi riêng lẻ, linh hoạt, nhưng chúng không tốt khi có một cách tiếp cận duy nhất, tiêu chuẩn và có thể tái sử dụng cho một vấn đề.

Mẫu Result cung cấp một cách tiêu chuẩn, có thể tái sử dụng để trả về cả thành công cũng như nhiều loại phản hồi không thành công từ các dịch vụ .NET theo cách có thể dễ dàng ánh xạ tới các loại phản hồi API. Mặc dù gói [Tung.Result](https://www.nuget.org/packages/Tung.Result/) không phụ thuộc vào ASP.NET Core và có thể được sử dụng từ bất kỳ ứng dụng .NET Core nào, gói đồng hành [Tung.Result.AspNetCore](https://www.nuget.org/packages/Tung.Result.AspNetCore/) bao gồm các tài nguyên để nâng cao việc sử dụng mẫu này trong các ứng dụng API web ASP.NET Core.

## Cách sử dụng mẫu

### Tạo kết quả (Result)

[Thư mục mẫu](https://github.com/tung-md/Result/tree/main/sample/Tung.Result.SampleWeb) bao gồm một số ví dụ về cách sử dụng dự án. Dưới đây là một vài cách sử dụng đơn giản.

Hãy tưởng tượng đoạn mã bên dưới được xác định trong một dịch vụ miền truy xuất WeatherForecasts. Khi so sánh với phương pháp được mô tả ở trên, phương pháp này sử dụng kết quả để xử lý các tình huống lỗi phổ biến như thiếu dữ liệu được biểu thị là Không tìm thấy và/hoặc lỗi xác thực đầu vào được biểu thị là Không hợp lệ. Nếu thực thi thành công, kết quả sẽ chứa dữ liệu ngẫu nhiên được tạo bởi câu lệnh return cuối cùng.

```csharp
public Result<IEnumerable<WeatherForecast>> GetForecast(ForecastRequestDto model)
{
    if (model.PostalCode == "NotFound") return Result<IEnumerable<WeatherForecast>>.NotFound();

    // validate model
    if (model.PostalCode.Length > 10)
    {
        return Result<IEnumerable<WeatherForecast>>.Invalid(new List<ValidationError> {
            new ValidationError
            {
                Identifier = nameof(model.PostalCode),
                ErrorMessage = "PostalCode cannot exceed 10 characters." 
            }
        });
    }

    var rng = new Random();
    return new Result<IEnumerable<WeatherForecast>>(Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
    .ToArray());
}
```

### Chuyển Results sang ActionResults

Tiếp tục với ví dụ về dịch vụ miền ở phần trước, điều quan trọng là phải chỉ ra rằng dịch vụ miền không biết về `ActionResult` các loại MVC/v.v. khác. Nhưng vì nó đang sử dụng `Result<T>` nên nó có thể trả về các kết quả dễ dàng được ánh xạ tới mã trạng thái HTTP. Lưu ý rằng phương thức trên trả về một `Result<IEnumerable<WeatherForecast>`nhưng trong một số trường hợp, nó có thể cần trả về một `Invalid` kết quả hoặc một `NotFound` kết quả. Nếu không, nó sẽ trả về `Success` kết quả có giá trị trả về thực tế (giống như API sẽ trả về HTTP 200 và kết quả thực tế của lệnh gọi API).

Bạn có thể áp dụng `[TranslateResultToActionResult]` thuộc tính cho [API Endpoint](https://github.com/tung-md/ApiEndpoints) (hoặc controller action nếu bạn vẫn sử dụng nó) và nó sẽ tự động dịch kiểu trả về `Result<T>` của phương thức sang loại `ActionResult<T>` phù hợp dựa trên loại Result.

```csharp
[TranslateResultToActionResult]
[HttpPost("Create")]
public Result<IEnumerable<WeatherForecast>> CreateForecast([FromBody]ForecastRequestDto model)
{
    return _weatherService.GetForecast(model);
}
```

Ngoài ra, bạn có thể sử dụng phương thức trợ giúp `ToActionResult` trong điểm cuối để đạt được điều tương tự:

```csharp
[HttpPost("/Forecast/New")]
public override ActionResult<IEnumerable<WeatherForecast>> Handle(ForecastRequestDto request)
{
    return this.ToActionResult(_weatherService.GetForecast(request));

    // alternately
    // return _weatherService.GetForecast(request).ToActionResult(this);
}
```

### Chuyển Results sang Minimal API Results 

Tương tự như cách `ToActionResult` phương thức mở rộng dịch `Tung.Results` sang `ActionResults`, `ToMinimalApiResult` kết quả dịch sang các loại mới `Microsoft.AspNetCore.Http.Results` `IResult` trong .NET 6+. Đoạn mã sau đây minh họa cách người ta có thể sử dụng dịch vụ miền trả về một `Result<IEnumerable<WeatherForecast>>` và chuyển đổi thành một phiên bản `Microsoft.AspNetCore.Http.Results`.

```csharp
app.MapPost("/Forecast/New", (ForecastRequestDto request, WeatherService weatherService) =>
{
    return weatherService.GetForecast(request).ToMinimalApiResult();
})
.WithName("NewWeatherForecast");
```

Bạn có thể tìm thấy mẫu API tối thiểu đầy đủ trong [thư mục mẫu](./sample/Tung.Result.SampleMinimalApi/Program.cs).

### Ánh xạ kết quả từ loại này sang loại khác

Trường hợp sử dụng phổ biến là ánh xạ giữa các thực thể miền với các loại phản hồi API thường được biểu thị dưới dạng DTO. Bạn có thể ánh xạ kết quả chứa thực thể miền tới Kết quả chứa DTO bằng cách sử dụng phương thức `Map` này. Ví dụ sau đây gọi phương thức `_weatherService.GetSingleForecast` trả về một `Result<WeatherForecast>` sau đó được chuyển đổi thành một `Result<WeatherForecastSummaryDto>` bằng lệnh gọi tới `Map`. Sau đó, kết quả được chuyển đổi thành `ActionResult<WeatherForecastSummaryDto>` sử dụng phương thức trợ giúp `ToActionResult` .

```csharp
[HttpPost("Summary")]
public ActionResult<WeatherForecastSummaryDto> CreateSummaryForecast([FromBody] ForecastRequestDto model)
{
    return _weatherService.GetSingleForecast(model)
        .Map(wf => new WeatherForecastSummaryDto(wf.Date, wf.Summary))
        .ToActionResult(this);
}
```

## Siêu dữ liệu ASP.NET API

Theo mặc định, Asp Net Core và API Explorer không biết gì về `[TranslateResultToActionResult]` cũng như những gì đang làm. Để phản ánh hành vi `[TranslateResultToActionResult]` trong siêu dữ liệu do API Explorer tạo ra (sau đó được sử dụng bởi các công cụ như Swashbuckle, NSwag, v.v.), bạn có thể sử dụng `ResultConvention`:

```csharp
services.AddControllers(mvcOptions => mvcOptions.AddDefaultResultConvention());
```
Điều này sẽ bổ sung `[ProducesResponseType]` cho mọi `ResultStatus` điểm cuối đã biết được đánh dấu bằng `[TranslateResultToActionResult]`. Để tùy chỉnh hành vi của ResultConvention, người ta có thể sử dụng phương thức `AddResultConvention`:

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
     ));
```

Mã này có chức năng tương đương với ví dụ trước.

Từ đây bạn có thể sửa đổi ánh xạ ResultStatus thành HttpStatusCode

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
            .For("POST", HttpStatusCode.Created)
            .For("DELETE", HttpStatusCode.NoContent))
        .For(ResultStatus.Error, HttpStatusCode.InternalServerError)
    ));
```
`ResultConvention` sẽ thêm `[ProducesResponseType]` cho mọi trạng thái kết quả được định cấu hình trong `ResultStatusMap`. `AddDefaultMap()` ánh xạ mọi Loại kết quả (ResultType) đã biết, vì vậy nếu bạn muốn loại trừ ResultType nhất định khỏi danh sách (ví dụ: ứng dụng của bạn không có xác thực và ủy quyền và bạn không muốn 401 và 403 được liệt kê dưới dạng `SupportedResponseType`), bạn có thể thực hiện việc này:

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
            .For("POST", HttpStatusCode.Created)
            .For("DELETE", HttpStatusCode.NoContent))
        .Remove(ResultStatus.Forbidden)
        .Remove(ResultStatus.Unauthorized)
    ));
```

Ngoài ra, bạn có thể chỉ định trạng thái kết quả (lỗi) nào được mong đợi từ một điểm cuối nhất định:

```csharp
[TranslateResultToActionResult()]
[ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
[HttpDelete("Remove/{id}")]
public Result RemovePerson(int id)
{
    // Method logic
}
```
`!!!` Nếu bạn liệt kê một trạng thái Kết quả nhất định trong `ExpectedFailures`, trạng thái đó phải được định cấu hình `ResultConvention` khi khởi động.

Một tính năng có thể định cấu hình khác là phần nào của đối tượng Kết quả được trả về trong trường hợp có lỗi cụ thể:

```csharp
services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Error, HttpStatusCode.BadRequest, resultStatusOptions => resultStatusOptions
            .With((ctrlr, result) => string.Join("\r\n", result.ValidationErrors)))
    ));
```

### Sử dụng Results với FluentValidation

Chúng ta có thể sử dụng Tung.Result.FluentValidation trên một dịch vụ có FluentValidation như thế này:

```csharp
public async Task<Result<BlogCategory>> UpdateAsync(BlogCategory blogCategory)
{
    if (Guid.Empty == blogCategory.BlogCategoryId) return Result<BlogCategory>.NotFound();

    var validator = new BlogCategoryValidator();
    var validation = await validator.ValidateAsync(blogCategory);
    if (!validation.IsValid)
    {
        return Result<BlogCategory>.Invalid(validation.AsErrors());
    }

    var itemToUpdate = (await GetByIdAsync(blogCategory.BlogCategoryId)).Value;
    if (itemToUpdate == null)
    {
        return Result<BlogCategory>.NotFound();
    }

    itemToUpdate.Update(blogCategory.Name, blogCategory.ParentId);

    return Result<BlogCategory>.Success(await _blogCategoryRepository.UpdateAsync(itemToUpdate));
}
```

## Bắt đầu

Nếu bạn đang xây dựng API ASP.NET Core Web, bạn chỉ cần cài đặt gói [Tung.Result.AspNetCore](https://www.nuget.org/packages/Tung.Result.AspNetCore/) để bắt đầu. Sau đó, áp dụng thuộc tính `[TranslateResultToActionResult]` cho bất kỳ hành động hoặc bộ điều khiển nào mà bạn muốn tự động chuyển từ loại Result sang loại ActionResult.
