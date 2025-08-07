'use client'

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import styles from "@/styles/recipts-page/hero/hero.module.css";
import { getInactiveUnits } from '@/services/api/unitClient';

interface Unit{
    id:number;
    name:string;
}

export default function Hero() {
  const router = useRouter();
  const [archiveUnits, setArchiveUnits] = useState<Unit[]>([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await getInactiveUnits();
        setArchiveUnits(data);
      } catch (error) {
        console.error(error);
      }
    }

    fetchData();
  }, []);
  return (
    <main className={styles.mainContainer}>
        <h1 className={styles.headerText}>Единицы измерения</h1>
        <div className={styles.buttonGroup}>
          <button className={styles.acceptButton} onClick={() => router.push('/unit-page/')}>К рабочим</button>
        </div>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Наименование</th>
            </tr>
          </thead>
          <tbody>
            {archiveUnits.map(unit => (
              <tr key={unit.id} onClick={() => router.push(`/edit-archiveunit/${unit.id}`)}>
                <td>{unit.name}</td>
              </tr>
            ))}
          </tbody>
        </table>
    </main>
  );
}