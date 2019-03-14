import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'Thống kê',
    icon: 'nb-bar-chart',
    link: '/pages/dashboard',
    home: true,
  },
  {
    title: 'Quản kho',
    icon: 'nb-home',
    children: [
      {
        title: 'Kho',
        link: '/pages/warehouse/list',
      },
      {
        title: 'Hoạt động',
        link: '/pages/warehouse/log',
      },
      {
        title: 'Nhập kho',
        link: '/pages/warehouse/import',
      },
      {
        title: 'Xuất kho',
        link: '/pages/warehouse/export',
      },
      {
        title: 'Chuyển kho',
        link: '/pages/warehouse/transfer',
      }
    ]
  },
  {
    title: 'Công nợ',
    icon: 'nb-compose',
    children: [
      {
        title: 'DS phiếu ghi nợ',
        link: '/pages/debtnote/list',
      },
      {
        title: 'DS phiếu thu nợ',
        link: '/pages/debtnote/listdebtcollect',
      },
      {
        title: 'Tạo ghi công nợ',
        link: '/pages/debtnote/create',
      },
      {
        title: 'Tạo thu công nợ',
        link: '/pages/debtnote/createcollectdebtnote',
      }
    ]
  },
  {
    title: 'Sản phẩm',
    icon: 'nb-grid-a',
    children: [
      {
        title: 'Thêm sản phẩm',
        link: '/pages/product/create',
      }
    ]
  },
  {
    title: 'Đăng xuất',
    icon: 'nb-power',
    link: '/auth/login/'
  }
];
