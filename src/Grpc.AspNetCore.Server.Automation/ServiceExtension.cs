// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grpc.AspNetCore.Server.Automation.Extension.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Grpc.AspNetCore.Server.Automation
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceExtension
    {
        #region 添加自动注入Grpc节点

        /// <summary>
        /// 添加自动注入Grpc节点
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="endpoints"></param>
        public static void AddGrpcEndpoint(this IServiceProvider serviceProvider, IEndpointRouteBuilder endpoints)
        {
            Type workerType = typeof(GrpcEndpointRouteBuilderExtensions);
            MethodInfo staticDoWorkMethod = workerType.GetMethod("MapGrpcService");
            serviceProvider.GetService<ICollection<IGrpcService>>().ToList().ForEach(
                type =>
                {
                    if (staticDoWorkMethod != null && type.GetType().IsClass && !type.GetType().IsAbstract)
                    {
                        MethodInfo curMethod = staticDoWorkMethod.MakeGenericMethod(type.GetType());
                        curMethod.Invoke(null, new[] {endpoints}); //Static method
                    }
                });
        }

        #endregion

        #region 添加Grpc健康检查

        /// <summary>
        /// 添加Grpc健康检查
        /// </summary>
        /// <param name="endpoints">节点</param>
        /// <param name="checkUrl">检查地址</param>
        public static void AddGrpcHealthy(this IEndpointRouteBuilder endpoints, string checkUrl)
        {
            if (!string.IsNullOrEmpty(checkUrl))
            {
                checkUrl = "/Check/Healthy";
            }

            endpoints.MapGet(checkUrl,
                async context => { await context.Response.WriteAsync("Ok!"); });
        }

        #endregion

        #region 使用Grpc

        /// <summary>
        /// 使用Grpc
        /// </summary>
        /// <param name="app"></param>
        /// <param name="endpoints">节点</param>
        /// <param name="action"></param>
        public static void UseGrpc(this IApplicationBuilder app, IEndpointRouteBuilder endpoints = null,
            Action<IEndpointRouteBuilder> action = null)
        {
            if (endpoints == null)
            {
                app.UseEndpoints(item =>
                {
                    app.ApplicationServices.AddGrpcEndpoint(item);
                    action?.Invoke(item);
                });
            }
            else
            {
                app.ApplicationServices.AddGrpcEndpoint(endpoints);
            }
        }

        #endregion
    }
}