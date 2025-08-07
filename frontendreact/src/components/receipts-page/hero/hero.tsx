'use client'

import { useRouter } from 'next/navigation';
import { useState } from 'react';
import styles from '@/styles/recipts-page/hero/hero.module.css';

export default function Hero() {
    const router = useRouter();
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [selectedNumbers, setSelectedNumbers] = useState<string[]>([]);
    const [selectedResources, setSelectedResources] = useState<string[]>([]);
    const [selectedUnits, setSelectedUnits] = useState<string[]>([]);

    const receiptNumbers = [';', '125151', '1', '676', '888', '4679'];
    const resources = ['Ноутбуки111', 'рргрл', 'новый ресурс', 'Ресурс12'];
    const units = ['кг', 'литр', 'Amogus'];

    const mockReceipts = [
        { id: 1, number: '125151', date: '2025-07-30', resource: 'Ноутбуки111', unit: 'кг', quantity: 6 },
        { id: 2, number: '888', date: '2025-08-01', resource: 'Ресурс12', unit: 'шт', quantity: 3 },
    ];

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
                    value={selectedResources}
                    onChange={(e) => setSelectedResources(Array.from(e.target.selectedOptions, o => o.value))}
                >
                    {resources.map(r => (
                        <option key={r} value={r}>{r}</option>
                    ))}
                </select>

                <select
                    multiple
                    className={styles.select}
                    value={selectedUnits}
                    onChange={(e) => setSelectedUnits(Array.from(e.target.selectedOptions, o => o.value))}
                >
                    {units.map(u => (
                        <option key={u} value={u}>{u}</option>
                    ))}
                </select>
            </div>

            <div className={styles.buttonGroup}>
                <button className={styles.acceptButton}>Применить</button>
                <button className={styles.addButton} onClick={() => router.push('/receipts/create')}>Добавить</button>
            </div>
            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>Номер</th>
                        <th>Дата</th>
                        <th>Ресурс</th>
                        <th>Ед. изм.</th>
                        <th>Кол-во</th>
                    </tr>
                </thead>
                <tbody>
                    {mockReceipts.map(receipt => (
                        <tr key={receipt.id} onClick={() => router.push(`/receipts-page/${receipt.id}`)}>
                            <td>{receipt.number}</td>
                            <td>{receipt.date}</td>
                            <td>{receipt.resource}</td>
                            <td>{receipt.unit}</td>
                            <td>{receipt.quantity}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </main>
    );
}
