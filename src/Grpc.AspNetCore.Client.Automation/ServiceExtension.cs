// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using EInfrastructure.Core.Tools.Systems;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Grpc.AspNetCore.Client.Automation
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceExtension
    {
        #region 添加GRpcClient自动注入

        /// <summary>
        /// 添加GRpcClient自动注入
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void AddGrpcClient(this IServiceCollection serviceProvider)
        {
            var gRpcClientList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x =>
                x.GetTypes().Where(y => y.GetInterfaces().Contains(typeof(ClientBase)))).ToList();
            gRpcClientList.ForEach(type =>
            {
                var clientBase = AssemblyCommon.CreateInstance(type) as ClientBase;
                serviceProvider.AddGrpcClient<>(clientBase,);
                serviceProvider.AddGrpcClient<>(gRpcClient);
            });
        }

        #endregion
    }
}