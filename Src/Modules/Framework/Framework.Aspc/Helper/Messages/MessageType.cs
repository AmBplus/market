namespace Infrastructure.Messages;

/// <summary>
/// Version 3.0
/// </summary>
public enum MessageType : byte
{
	PageError,
	PageWarning,
	PageSuccess,
	PageInfo,

	ToastError,
	ToastWarning,
	ToastSuccess,
        ToastInfo,

	AlertError,
	AlertWarning,
	AlertSuccess,
        AlertInfo,

}
