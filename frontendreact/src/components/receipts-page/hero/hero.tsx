'use client'

import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from '@/styles/recipts-page/hero/hero.module.css';

import { getResources } from '@/services/api/resourceClient';
import { getUnits } from '@/services/api/unitClient';
import { getReceipts, getFilteredReceipts, ReceiptDto, ReceiptFilter } from '@/services/api/receiptClient';

interface ReceiptResource {
    id: number;
    resourceId: number;
    resourceName: string;
    unitId: number;
    unitName: string;
    quantity: number;
    receiptDocumentId: number;
}

export default function Hero() {
    const router = useRouter();
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const [selectedNumbers, setSelectedNumbers] = useState<string[]>([]);
    const [selectedResourceIds, setSelectedResourceIds] = useState<string[]>([]);
    const [selectedUnitIds, setSelectedUnitIds] = useState<string[]>([]);

    const [receiptNumbers, setReceiptNumbers] = useState<string[]>([]);
    const [resources, setResources] = useState<{ id: number; name: string }[]>([]);
    const [units, setUnits] = useState<{ id: number; name: string }[]>([]);
    const [receipts, setReceipts] = useState<ReceiptDto[]>([]);

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadData() {
            try {
                const [resourcesData, unitsData, receiptsData] = await Promise.all([
                    getResources(),
                    getUnits(),
                    getReceipts()
                ]);

                console.log('Загруженные данные:', { resourcesData, unitsData, receiptsData });

                setResources(resourcesData);
                setUnits(unitsData);
                setReceiptNumbers(receiptsData.map(r => r.number));
                setReceipts(receiptsData);
            } catch (err) {
                console.error('Ошибка при загрузке данных:', err);
            } finally {
                setLoading(false);
            }
        }
        loadData();
    }, []);

    const handleApplyFilters = async () => {
        setLoading(true);
        try {
            const filter: ReceiptFilter = {};
            
            if (startDate) filter.startDate = startDate;
            if (endDate) filter.endDate = endDate;
            if (selectedNumbers.length > 0) filter.documentNumbers = selectedNumbers;
            if (selectedResourceIds.length > 0) {
                filter.resourceIds = selectedResourceIds.map(id => parseInt(id, 10)).filter(id => id > 0);
            }
            if (selectedUnitIds.length > 0) {
                filter.unitIds = selectedUnitIds.map(id => parseInt(id, 10)).filter(id => id > 0);
            }

            const filteredReceipts = await getFilteredReceipts(filter);
            setReceipts(filteredReceipts);
        } catch (err) {
            console.error('Ошибка при применении фильтров:', err);
        } finally {
            setLoading(false);
        }
    };

    const handleResetFilters = async () => {
        setStartDate('');
        setEndDate('');
        setSelectedNumbers([]);
        setSelectedResourceIds([]);
        setSelectedUnitIds([]);
        
        setLoading(true);
        try {
            const allReceipts = await getReceipts();
            setReceipts(allReceipts);
        } catch (err) {
            console.error('Ошибка при сбросе фильтров:', err);
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return <div>Загрузка...</div>;
    }

    return (
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Поступления</h1>

            <div className={styles.filterText}>
                <h4 className={styles.headerText}>Период</h4>
                <h4 className={styles.headerText}>Номер поступления</h4>
                <h4 className={styles.headerText}>Ресурс</h4>
                <h4 className={styles.headerText}>Единица измерения</h4>
            </div>

            <div className={styles.filterBox}>
                <div className={styles.inputGroup}>
                    <input
                        type="date"
                        className={styles.dateStart}
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                    />
                    <input
                        type="date"
                        className={styles.dateStart}
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                    />
                </div>

                <select
                    multiple
                    className={styles.select}
                    value={selectedNumbers}
                    onChange={(e) => setSelectedNumbers(Array.from(e.target.selectedOptions, o => o.value))}
                >
                    {receiptNumbers.map(num => (
                        <option key={num} value={num}>{num}</option>
                    ))}
                </select>

                <select
                    multiple
                    className={styles.select}
                    value={selectedResourceIds}
                    onChange={(e) => setSelectedResourceIds(Array.from(e.target.selectedOptions, o => o.value))}
                >
                    {resources.map(r => (
                        <option key={r.id} value={r.id.toString()}>{r.name}</option>
                    ))}
                </select>

                <select
                    multiple
                    className={styles.select}
                    value={selectedUnitIds}
                    onChange={(e) => setSelectedUnitIds(Array.from(e.target.selectedOptions, o => o.value))}
                >
                    {units.map(u => (
                        <option key={u.id} value={u.id.toString()}>{u.name}</option>
                    ))}
                </select>
            </div>

            <div className={styles.buttonGroup}>
                <button className={styles.acceptButton} onClick={handleApplyFilters}>Применить</button>
                <button className={styles.acceptButton} onClick={handleResetFilters}>Сбросить</button>
                <button className={styles.addButton} onClick={() => router.push('/receipts/create')}>Добавить</button>
            </div>

            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>Номер</th>
                        <th>Дата</th>
                        <th>Ресурс</th>
                        <th>Единица измерения</th>
                        <th>Количество</th>
                    </tr>
                </thead>
                <tbody>
                    {receipts.length === 0 ? (
                        <tr>
                            <td colSpan={5} style={{ textAlign: 'center' }}>Нет данных для отображения</td>
                        </tr>
                    ) : (
                        receipts.map((receipt) => (
                            <tr
                                key={receipt.id}
                                onClick={() => router.push(`/receipts-page/${receipt.id}`)}
                                style={{ cursor: 'pointer' }}
                            >
                                <td>{receipt.number}</td>
                                <td>{new Date(receipt.date).toLocaleDateString('ru-RU')}</td>
                                <td>
                                    {receipt.receiptResources.length === 0 ? (
                                        '-'
                                    ) : (
                                        receipt.receiptResources.map((res, index) => (
                                            <div key={res.id}>
                                                {res.resourceName}
                                                {index < receipt.receiptResources.length - 1 && <br />}
                                            </div>
                                        ))
                                    )}
                                </td>
                                <td>
                                    {receipt.receiptResources.length === 0 ? (
                                        '-'
                                    ) : (
                                        receipt.receiptResources.map((res, index) => (
                                            <div key={res.id}>
                                                {res.unitName}
                                                {index < receipt.receiptResources.length - 1 && <br />}
                                            </div>
                                        ))
                                    )}
                                </td>
                                <td>
                                    {receipt.receiptResources.length === 0 ? (
                                        '-'
                                    ) : (
                                        receipt.receiptResources.map((res, index) => (
                                            <div key={res.id}>
                                                {res.quantity}
                                                {index < receipt.receiptResources.length - 1 && <br />}
                                            </div>
                                        ))
                                    )}
                                </td>
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
        </main>
    );
}
