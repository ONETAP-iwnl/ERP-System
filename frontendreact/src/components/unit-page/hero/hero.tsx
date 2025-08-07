'use client'

import { useRouter } from 'next/navigation';
import { useState } from "react"
import styles from "@/styles/recipts-page/hero/hero.module.css"

export default function Hero() {
    const router = useRouter();
    const mockUnits = [
        {id:1, resourceName:'о\с (орешки в секунду)'}
    ]
    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурсы</h1>
             <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={() => router.push('/resource/create')}>Добавить</button>
                <button className={styles.archiveButton} onClick={() => router.push('/resource/archive')}>К архиву</button>
            </div>
            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>Наименование</th>
                    </tr>
                </thead>
                <tbody>
                    {mockUnits.map(units=> (
                        <tr key={units.id} onClick={() => router.push(`/edit-resource/${units.id}`)}>
                            <td>{units.resourceName}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </main>
    )
}