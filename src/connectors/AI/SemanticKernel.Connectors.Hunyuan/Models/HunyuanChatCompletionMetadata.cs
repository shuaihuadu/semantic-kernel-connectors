namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

/// <summary>
/// Represents the metadata of a Hunyuan chat completion.
/// </summary>
public sealed class HunyuanChatCompletionMetadata : ReadOnlyDictionary<string, object?>
{
    internal HunyuanChatCompletionMetadata() : base(new Dictionary<string, object?>()) { }

    private HunyuanChatCompletionMetadata(IDictionary<string, object?> dictionary) : base(dictionary) { }

    /// <summary>
    /// Unix 时间戳，单位为秒。
    /// </summary>
    public long? Created
    {
        get => (this.GetValueFromDictionary(nameof(this.Created)) as long?);
        internal init => this.SetValueInDictionary(value, nameof(this.Created));
    }

    /// <summary>
    /// Token 统计信息。
    /// 按照总 Token 数量计费。
    /// </summary>
    public Usage? Usage
    {
        get => (this.GetValueFromDictionary(nameof(this.Usage)) as Usage);
        internal init => this.SetValueInDictionary(value, nameof(this.Usage));
    }

    /// <summary>
    /// 免责声明。
    /// </summary>
    public string? Note
    {
        get => (this.GetValueFromDictionary(nameof(this.Note)) as string);
        internal init => this.SetValueInDictionary(value, nameof(this.Note));
    }

    /// <summary>
    /// 本轮对话的 ID。
    /// </summary>
    public string? Id
    {
        get => (this.GetValueFromDictionary(nameof(this.Id)) as string);
        internal init => this.SetValueInDictionary(value, nameof(this.Id));
    }

    /// <summary>
    /// 错误信息。
    /// 如果流式返回中服务处理异常，返回该错误信息。
    /// 注意：此字段可能返回 null，表示取不到有效值。
    /// </summary>
    public ErrorMsg? ErrorMsg
    {
        get => (this.GetValueFromDictionary(nameof(this.ErrorMsg)) as ErrorMsg);
        internal init => this.SetValueInDictionary(value, nameof(this.ErrorMsg));
    }

    /// <summary>
    /// 唯一请求 ID，由服务端生成，每次请求都会返回（若请求因其他原因未能抵达服务端，则该次请求不会获得 RequestId）。
    /// 定位问题时需要提供该次请求的 RequestId。本接口为流式响应接口，当请求成功时，RequestId 会被放在 HTTP 响应的 Header "X-TC-RequestId" 中。
    /// </summary>
    public string? RequestId
    {
        get => (this.GetValueFromDictionary(nameof(this.RequestId)) as string);
        internal init => this.SetValueInDictionary(value, nameof(this.RequestId));
    }

    /// <summary>
    /// 结束标志位，可能为 stop 或 sensitive。 stop 表示输出正常结束，sensitive 只在开启流式输出审核时会出现，表示安全审核未通过。
    /// </summary>
    public string? FinishReason
    {
        get => (this.GetValueFromDictionary(nameof(this.FinishReason)) as string);
        internal init => this.SetValueInDictionary(value, nameof(this.FinishReason));
    }

    /// <summary>
    /// 增量返回值，流式调用时使用该字段。 注意：此字段可能返回 null，表示取不到有效值。
    /// </summary>
    public Delta? Delta
    {
        get => (this.GetValueFromDictionary(nameof(this.Delta)) as Delta);
        internal init => this.SetValueInDictionary(value, nameof(this.Delta));
    }

    /// <summary>
    /// Converts a dictionary to a <see cref="HunyuanChatCompletionMetadata"/> object.
    /// </summary>
    public static HunyuanChatCompletionMetadata FromDictionary(IReadOnlyDictionary<string, object?> dictionary) => dictionary switch
    {
        null => throw new ArgumentNullException(nameof(dictionary)),
        HunyuanChatCompletionMetadata metadata => metadata,
        IDictionary<string, object?> metadata => new HunyuanChatCompletionMetadata(metadata),
        _ => new HunyuanChatCompletionMetadata(dictionary.ToDictionary(pair => pair.Key, pair => pair.Value))
    };

    private void SetValueInDictionary(object? value, string propertyName) => this.Dictionary[propertyName] = value;

    private object? GetValueFromDictionary(string propertyName) => this.Dictionary.TryGetValue(propertyName, out var value) ? value : null;
}
