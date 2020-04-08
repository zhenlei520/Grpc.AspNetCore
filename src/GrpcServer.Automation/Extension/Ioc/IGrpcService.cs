// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EInfrastructure.Core.Configuration.Ioc;

namespace Grpc.AspNetCore.Server.Automation.Extension.Ioc
{
    /// <summary>
    /// grpc服务需要依赖注入此接口
    /// </summary>
    public interface IGrpcService : IPerRequest
    {
    }
}