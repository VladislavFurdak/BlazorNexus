using Microsoft.AspNetCore.Components;

namespace UnitTests.Data;

[Route("/")]
public class BasePage : ComponentBase
{
}

[Route("/about")]
public class AboutPage : ComponentBase
{
}

[Route("/about/me")]
public class AboutMePage : ComponentBase
{
}

[Route("/product/{productId}")]
public class ProductPage : ComponentBase
{
}

[Route("/productext/{productId?}")]
public class ProductExtPage : ComponentBase
{
}

[Route("/dashboard/{areaId:int}/personal/{chunkId:guid?}")]
public class DashboardPage : ComponentBase
{
}

[Route("/levels/{active:bool?}/{dob:datetime?}/{price:decimal?}/{weight:double?}/{weight2:float?}/{id:guid?}/{ids:int?}/{ticks:long?}")]
public class ManyOptionalParamsPage : ComponentBase
{
}