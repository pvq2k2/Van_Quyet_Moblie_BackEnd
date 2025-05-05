namespace Van_Quyet_Moblie_BackEnd.Enums
{
    public enum StatusOrder
    {

        /// <summary> "Mới tạo đơn hàng" </summary>
        ready_to_pick = 1,

        /// <summary> "Nhân viên đang lấy hàng" </summary>
        picking = 2,

        /// <summary> "Hủy đơn hàng" </summary>
        cancel = 3,

        /// <summary> "Đang thu tiền người gửi" </summary>
        money_collect_picking = 4,

        /// <summary> "Nhân viên đã lấy hàng" </summary>
        picked = 5,

        /// <summary> "Hàng đang nằm ở kho" </summary>
        storing = 6,

        /// <summary> "Đang luân chuyển hàng" </summary>
        transporting = 7,

        /// <summary> "Đang phân loại hàng hóa" </summary>
        sorting = 8,

        /// <summary> "Nhân viên đang giao cho người nhận" </summary>
        delivering = 9,

        /// <summary> "Nhân viên đang thu tiền người nhận" </summary>
        money_collect_delivering = 10,

        /// <summary> "Nhân viên đã giao hàng thành công" </summary>
        delivered = 11,

        /// <summary> "Nhân viên giao hàng thất bại" </summary>
        delivery_fail = 12,

        /// <summary> "Đang đợi trả hàng về cho người gửi" </summary>
        waiting_to_return = 13,

        /// <summary> "Trả hàng" </summary>
        return_ = 14,

        /// <summary> "Đang luân chuyển hàng trả" </summary>
        return_transporting = 15,

        /// <summary> "Đang phân loại hàng trả" </summary>
        return_sorting = 16,

        /// <summary> "Nhân viên đang đi trả hàng" </summary>
        returning = 17,

        /// <summary> "Nhân viên trả hàng thất bại" </summary>
        return_fail = 18,

        /// <summary> "Nhân viên trả hàng thành công" </summary>
        returned = 19,

        /// <summary> "Đơn hàng ngoại lệ không nằm trong quy trình" </summary>
        exception = 20,

        /// <summary> "Hàng bị hư hỏng" </summary>
        damage = 21,

        /// <summary> "Hàng bị mất" </summary>
        lost = 22,
    }
}
