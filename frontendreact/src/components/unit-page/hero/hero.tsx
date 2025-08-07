'use client'

import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from "@/styles/recipts-page/hero/hero.module.css"
import { getUnits } from '@/services/api/unitClient';

interface Unit{
    id:number;
    name:string;
}

export default function Hero() {
    const router = useRouter();
    const [units, setUnits] = useState<Unit[]>([]);
    const [loading, setLoading] = useState(true);
    useEffect(() => {
    async function fetchUnits() {
        try {
            const data = await getUnits();
            setUnits(data);
        } catch (error) {
            console.error('Ошибка при загрузке единиц измерения:', error);
        } finally {
            setLoading(false);
        }
    }
    fetchUnits();
  }, []);

    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Единицы измерения</h1>
             <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={() => router.push('/unit/create')}>Добавить</button>
                <button className={styles.archiveButton} onClick={() => router.push('/unit/archive')}>К архиву</button>
            </div>
            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>Наименование</th>
                    </tr>
                </thead>
                <tbody>
                    {units.map((unit) => (
                        <tr key={unit.id} onClick={() => router.push(`/edit-unit/${unit.id}`)}>
                        <td>{unit.name}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </main>
    )
}