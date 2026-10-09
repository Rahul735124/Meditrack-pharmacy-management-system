import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { saveAs } from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

interface SalesReportItem {
  drugName: string;
  quantity: number;
  pricePerUnit: number;
  itemTotalPrice: number;
}

interface SalesReportOrder {
  orderId: number;
  orderDate: string;
  doctorName: string;
  doctorEmail: string;
  doctorContact: string;
  totalOrderAmount: number;
  items: SalesReportItem[];
}


@Component({
    selector: 'app-sales-report',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './sales-report.component.html',
})
export class SalesReportComponent implements OnInit {
    salesData: SalesReportOrder[] = [];
    startDate: string = '';
    endDate: string = '';
    reportGenerated = false;
    totalSales: number = 0;

    constructor(private adminService: AdminService) { }

    ngOnInit(): void { }

    fetchSalesReport() {
        if (!this.startDate || !this.endDate) {
            alert('Please select both start and end dates');
            return;
        }

        this.adminService.getSalesReport(this.startDate, this.endDate).subscribe({
            next: (res) => {
                this.salesData = res.data.orders; // Keep it grouped by order
                this.totalSales = res.data.totalSales;
                this.reportGenerated = true;
            },
            error: () => {
                alert('Error fetching sales report');
                this.reportGenerated = true;
            }
        });
    }


    downloadCSV() {
        if (!this.salesData.length) return;

        let csv = 'Order ID,Order Date,Order Time,Doctor Name,Doctor Email,Doctor Contact,Drug Name,Quantity,Price Per Unit,Total Price\n';

        this.salesData.forEach(order => {
            order.items.forEach(item => {
                csv += `${order.orderId},${new Date(order.orderDate).toLocaleString()},${order.doctorName},${order.doctorEmail},${order.doctorContact},${item.drugName},${item.quantity},${item.pricePerUnit},${item.itemTotalPrice}\n`;
            });
        });

        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
        saveAs(blob, `SalesReport_${this.startDate}_to_${this.endDate}.csv`);
    }


    downloadPDF() {
        if (!this.salesData.length) return;

        const doc = new jsPDF();
        doc.setFontSize(16);
        doc.text(`Sales Report`, 14, 15);

        let y = 25;

        this.salesData.forEach((order, index) => {
            // Order details
            doc.setFontSize(12);
            doc.text(`Order ID: ${order.orderId}`, 14, y);
            doc.text(`Order Date: ${new Date(order.orderDate).toLocaleString()}`, 14, y + 6);
            doc.text(`Doctor: ${order.doctorName}`, 14, y + 12);
            doc.text(`Email: ${order.doctorEmail}`, 14, y + 18);
            doc.text(`Contact: ${order.doctorContact}`, 14, y + 24);
            doc.text(`Total Amount: Rs. ${order.totalOrderAmount}`, 14, y + 30);

            y += 36;

            // Items Table
            autoTable(doc, {
                startY: y,
                head: [['Drug Name', 'Quantity', 'Price Per Unit', 'Total Price']],
                body: order.items.map(item => [
                    item.drugName,
                    item.quantity,
                    `Rs. ${item.pricePerUnit}`,
                    `Rs. ${item.itemTotalPrice}`
                ]),
                theme: 'striped',
                styles: { fontSize: 10 },
            });

            y = (doc as any).lastAutoTable.finalY + 10;

            // Check for page break
            if (y > 260 && index !== this.salesData.length - 1) {
                doc.addPage();
                y = 15;
            }
        });

        doc.save(`SalesReport_${this.startDate}_to_${this.endDate}.pdf`);
    }

}
