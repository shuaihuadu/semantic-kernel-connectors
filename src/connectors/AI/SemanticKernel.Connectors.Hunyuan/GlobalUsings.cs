﻿// Copyright (c) IdeaTech. All rights reserved.

global using System.Collections.ObjectModel;
global using System.Diagnostics.Metrics;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.Json;
global using IdeaTech.SemanticKernel.Connectors.Hunyuan;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using Microsoft.SemanticKernel;
global using Microsoft.SemanticKernel.ChatCompletion;
global using Microsoft.SemanticKernel.Diagnostics;
global using Microsoft.SemanticKernel.Embeddings;
global using Microsoft.SemanticKernel.Memory;
global using Microsoft.SemanticKernel.Services;
global using Microsoft.SemanticKernel.Text;
global using Microsoft.SemanticKernel.TextGeneration;
global using TencentCloud.Common;
global using TencentCloud.Common.Profile;
global using TencentCloud.Hunyuan.V20230901;
global using TencentCloud.Hunyuan.V20230901.Models;
